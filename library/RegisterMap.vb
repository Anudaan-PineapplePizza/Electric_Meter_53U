Option Strict On
Option Explicit On

Imports System.Drawing
Imports System
Imports System.Runtime.InteropServices

' =============================================================================
' RegisterMap.vb
' Source de registres Modbus du compteur 53U.
' Toute modification d'adresse, facteur, nom, couleur ou plage se fait ICI.
'
' Les autres fichiers (ModbusMaster, Live_Chart, Window) consomment ce module.
' =============================================================================

' ─────────────────────────────────────────────────────────────────────────────
' ENUM : identifiant de chaque signal (valeurs 0-45)
' ─────────────────────────────────────────────────────────────────────────────
Public Enum SignalID As Integer
    ' ── Valeurs totales (des 3 phases) (IDs 0-17)  ───────────────────────────
    U = 0           ' Tension totale efficace
    I = 1           ' Courant total efficace
    P = 2           ' Puissance active totale
    Q = 3           ' Puissance reactive totale
    S = 4           ' Puissance apparente totale
    PF = 5          ' Facteur de puissance total
    F = 6           ' Frequence
    Phase_Dir = 7   ' Direction phase (+1=LEAD=capacitif, -1=LAG=inductif)
    ' ── THD ──────────────────────────────────────────────────────────────────
    THDi1 = 8
    THDi2 = 9
    THDi3 = 10
    THDiN = 11
    THDu12 = 12
    THDu23 = 13
    THDu31 = 14
    THDu1N = 15
    THDu2N = 16
    THDu3N = 17
    ' ── Courants par phase ───────────────────────────────────────────────────
    I1 = 18
    I2 = 19
    I3 = 20
    I_Neutral = 21
    ' ── Tensions delta ───────────────────────────────────────────────────────
    U12 = 22
    U23 = 23
    U31 = 24
    ' ── Tensions phase (ligne-neutre) ────────────────────────────────────────
    U1N = 25
    U2N = 26
    U3N = 27
    ' ── Puissances par phase ─────────────────────────────────────────────────
    P1 = 28
    P2 = 29
    P3 = 30
    Q1 = 31
    Q2 = 32
    Q3 = 33
    S1 = 34
    S2 = 35
    S3 = 36
    ' ── Facteur de puissance par phase ───────────────────────────────────────
    PF1 = 37
    PF2 = 38
    PF3 = 39
    ' ── Direction et angles (lecture seule, non traces en temps reel) ────────
    DIR1 = 40
    DIR2 = 41
    DIR3 = 42
    UT12 = 43
    UT23 = 44
    UT31 = 45

    ' ── ePack TCP ────────────────────────────────────────────────────────────
    ePack_Vline = 46      '256 Tension de ligne
    ePack_Irms = 47       '257 Courant efficace de la charge
    ePack_Isqburst = 48   '258 Valeur moyenne du carré du courant de charge en train d'onde
    ePack_Isq = 49        '259 Valeur du carré du courant de charge
    ePack_Vrms = 50       '260 Tension efficace de la charge
    ePack_Vsq = 51        '261 Valeur du carré de la tension de charge
    ePack_Pburst = 52     '262 Mesure puissance réelle en train d'onde
    ePack_P = 53          '263 Mesure de la puissance réelle
    ePack_S = 54          '264 Mesure de la puissance apparente
    ePack_PF = 55         '265 Mesure du facteur de puissance
    ePack_Z = 56          '266 Impédance de charge
    ePack_Frequency = 57  '267 Fréquence
    ePack_Vsqburst = 58   '268 Valeur moyenne du carré de la tension de charge en train d'onde
    ePack_Ramping = 59    '1440 (0 = Ramping // 1 = Finished)
    ' ── ePack Config ─────────
    ePack_VNominal = 60
    ePack_INominal = 61
    ePack_Firing = 62
    ePack_Control = 63
    ePack_ILimit = 64
    ePack_I2Transfer = 65
    ePack_Xfmr = 66
    ePack_Heater = 67
    ePack_AiFct = 68
    ePack_AiType = 69
    ePack_Di1Fct = 70
    ePack_Di2Fct = 71
    '──────────────────────────────────────────────────────────────────────────
End Enum

' ──── ENUM Identifier le type de signal pour le mettre dans un Group ─────────
Public Enum SignalGroup As Integer
    Current = 0
    Voltage = 1
    Power = 2
    Harmonic = 3
    Phase = 4
    ePack = 5
End Enum

' ─────────────────────────────────────────────────────────────────────────────
' STRUCTURE : definition complete d'un registre
' ─────────────────────────────────────────────────────────────────────────────
Public Structure RegisterDef
    Public ID As SignalID
    Public Name As String             ' Nom affiché dans l'UI
    Public Unit As String             ' Unité physique apres conversion
    Public ScaleFactor As Double      ' raw / ScaleFactor = valeur physique
    Public MinVal As Double           ' Valeur min pour l'axe Y du graph
    Public MaxVal As Double           ' Valeur max pour l'axe Y
    Public PlotColor As Color         ' Couleur de la courbe
    Public Group As SignalGroup
    Public ModbusAddr As Integer      ' adresse base 0 (EasyModbus)
    Public WordCount As Integer       ' 1 = 16 bits, 2 = 32 bits signe
    Public IsRealTime As Boolean      ' trace sur le graphe live
    Public DefaultEnabled As Boolean  ' active par defaut au premier lancement

    Public Sub New(id As SignalID,
                   name As String,
                   unit As String,
                   scale As Double,
                   minV As Double,
                   maxV As Double,
                   color As Color,
                   group As SignalGroup,
                   addr As Integer,
                   words As Integer,
                   realTime As Boolean,
                   defltEnabled As Boolean)
        'defltEnabled As Boolean,
        'y1Cand As Boolean)
        Me.ID = id
        Me.Name = name
        Me.Unit = unit
        Me.ScaleFactor = scale
        Me.MinVal = minV
        Me.MaxVal = maxV
        Me.PlotColor = color
        Me.Group = group
        Me.ModbusAddr = addr
        Me.WordCount = words
        Me.IsRealTime = realTime
        Me.DefaultEnabled = defltEnabled

    End Sub
End Structure

' =============================================================================
' MODULE : table des registres + helpers
' =============================================================================
Public Module RegisterMap

    Public ReadOnly Property SIG_TOTAL_COUNT As Integer
        Get
            Return Marshal.SizeOf(GetType(SignalID))
        End Get
    End Property

    ' TABLE COMPLÈTE — lazy-initialized (necessaire pour Color.FromArgb dans Module)
    ' Indexee par SignalID (0-45)
    Private _registers As RegisterDef() = Nothing

    Public ReadOnly Property Registers As RegisterDef()
        Get
            If _registers Is Nothing Then _registers = BuildRegisters()
            Return _registers
        End Get
    End Property

    '─────────────── Création + Définition des registres ─────────────────────────
    ' Construit et retourne le tableau de tous les RegisterDef utilisés par l'application.
    ' Chaque entrée décrit un signal Modbus avec ses métadonnées d'affichage et d'acquisition.
    '
    ' Ordre des paramètres du constructeur RegisterDef :
    '   id            (SignalID)     – Identifiant enum unique du signal
    '   name          (String)       – Nom affiché dans l'UI (légende, tableau)
    '   unit          (String)       – Unité physique après conversion (ex: "V", "A", "W")
    '   scale         (Double)       – Diviseur : valeur physique = raw / scale
    '   minV          (Double)       – Valeur minimale pour l'axe Y du graphique
    '   maxV          (Double)       – Valeur maximale pour l'axe Y du graphique
    '   color         (Color)        – Couleur de la courbe sur le graphique temps réel
    '   group         (SignalGroup)  – Nature du signal de mesure (Voltage, Current, Power, Harmonic, Phase)
    '   addr          (Integer)      – Adresse Modbus base 0 (EasyModbus)
    '   words         (Integer)      – Taille des registres en WORD (16 bits) : 1 = HD, 2 = Autres
    '   realTime      (Boolean)      – True = signal tracé sur le graphe live
    '   defltEnabled  (Boolean)      – True = signal activé par défaut au premier lancement                                 (TO DO : Pointer la variable du .cfg correspondant)
    '─────────────────────────────────────────────────────────────────────────────
    Private Function BuildRegisters() As RegisterDef()
        Dim r As New List(Of RegisterDef)
        r.Add(New RegisterDef(SignalID.U, "Voltage", "V", 100.0, 0, 250, Color.DodgerBlue, SignalGroup.Voltage, 2, 2, True, True))
        r.Add(New RegisterDef(SignalID.I, "Current", "A", 1000.0, 0, 100, Color.FromArgb(0, 220, 150), SignalGroup.Current, 0, 2, True, True))
        r.Add(New RegisterDef(SignalID.P, "P Active", "W", 1.0, -50000, 100000, Color.FromArgb(255, 185, 50), SignalGroup.Power, 4, 2, True, True))
        r.Add(New RegisterDef(SignalID.Q, "Q React.", "VAR", 1.0, -50000, 100000, Color.FromArgb(255, 100, 50), SignalGroup.Power, 6, 2, True, True))
        r.Add(New RegisterDef(SignalID.S, "S Apprt.", "VA", 1.0, 0, 100000, Color.FromArgb(200, 130, 255), SignalGroup.Power, 8, 2, True, True))
        r.Add(New RegisterDef(SignalID.PF, "PF", "PF", 100.0, -1, 1, Color.FromArgb(255, 110, 110), SignalGroup.Power, 10, 2, True, True))
        r.Add(New RegisterDef(SignalID.F, "Frequency", "Hz", 100.0, 45, 65, Color.FromArgb(140, 200, 255), SignalGroup.Phase, 12, 2, True, True))
        r.Add(New RegisterDef(SignalID.Phase_Dir, "Phase Dir", "", 1.0, -1.5, 1.5, Color.FromArgb(255, 220, 0), SignalGroup.Phase, 14, 2, True, False))
        r.Add(New RegisterDef(SignalID.THDi1, "THDi 1", "%", 10.0, 0, 100, Color.FromArgb(0, 220, 120), SignalGroup.Harmonic, 1280, 2, True, True))
        r.Add(New RegisterDef(SignalID.THDi2, "THDi 2", "%", 10.0, 0, 100, Color.FromArgb(0, 190, 95), SignalGroup.Harmonic, 1282, 2, True, True))
        r.Add(New RegisterDef(SignalID.THDi3, "THDi 3", "%", 10.0, 0, 100, Color.FromArgb(0, 160, 70), SignalGroup.Harmonic, 1284, 2, True, True))
        r.Add(New RegisterDef(SignalID.THDiN, "THDi N", "%", 10.0, 0, 100, Color.FromArgb(0, 250, 170), SignalGroup.Harmonic, 1286, 2, True, False))
        r.Add(New RegisterDef(SignalID.THDu12, "THDu 1-2", "%", 10.0, 0, 100, Color.FromArgb(80, 160, 255), SignalGroup.Harmonic, 1288, 2, True, False))
        r.Add(New RegisterDef(SignalID.THDu23, "THDu 2-3", "%", 10.0, 0, 100, Color.FromArgb(50, 130, 235), SignalGroup.Harmonic, 1290, 2, True, False))
        r.Add(New RegisterDef(SignalID.THDu31, "THDu 3-1", "%", 10.0, 0, 100, Color.FromArgb(30, 100, 215), SignalGroup.Harmonic, 1292, 2, True, False))
        r.Add(New RegisterDef(SignalID.THDu1N, "THDu 1N", "%", 10.0, 0, 100, Color.FromArgb(110, 185, 255), SignalGroup.Harmonic, 1294, 2, True, False))
        r.Add(New RegisterDef(SignalID.THDu2N, "THDu 2N", "%", 10.0, 0, 100, Color.FromArgb(140, 210, 255), SignalGroup.Harmonic, 1296, 2, True, False))
        r.Add(New RegisterDef(SignalID.THDu3N, "THDu 3N", "%", 10.0, 0, 100, Color.FromArgb(170, 230, 255), SignalGroup.Harmonic, 1298, 2, True, False))
        r.Add(New RegisterDef(SignalID.I1, "Current L1", "A", 1000.0, 0, 100, Color.FromArgb(0, 200, 140), SignalGroup.Current, 32, 2, True, False))
        r.Add(New RegisterDef(SignalID.I2, "Current L2", "A", 1000.0, 0, 100, Color.FromArgb(0, 170, 120), SignalGroup.Current, 34, 2, True, False))
        r.Add(New RegisterDef(SignalID.I3, "Current L3", "A", 1000.0, 0, 100, Color.FromArgb(0, 140, 100), SignalGroup.Current, 36, 2, True, False))
        r.Add(New RegisterDef(SignalID.I_Neutral, "Current N", "A", 1000.0, 0, 100, Color.FromArgb(0, 110, 80), SignalGroup.Current, 38, 2, True, False))
        r.Add(New RegisterDef(SignalID.U12, "Voltage 1-2", "V", 100.0, 0, 300, Color.FromArgb(100, 180, 255), SignalGroup.Voltage, 40, 2, True, False))
        r.Add(New RegisterDef(SignalID.U23, "Voltage 2-3", "V", 100.0, 0, 300, Color.FromArgb(70, 150, 235), SignalGroup.Voltage, 42, 2, True, False))
        r.Add(New RegisterDef(SignalID.U31, "Voltage 3-1", "V", 100.0, 0, 300, Color.FromArgb(40, 120, 215), SignalGroup.Voltage, 44, 2, True, False))
        r.Add(New RegisterDef(SignalID.U1N, "Voltage 1N", "V", 100.0, 0, 300, Color.FromArgb(130, 200, 255), SignalGroup.Voltage, 46, 2, True, False))
        r.Add(New RegisterDef(SignalID.U2N, "Voltage 2N", "V", 100.0, 0, 300, Color.FromArgb(150, 215, 255), SignalGroup.Voltage, 48, 2, True, False))
        r.Add(New RegisterDef(SignalID.U3N, "Voltage 3N", "V", 100.0, 0, 300, Color.FromArgb(170, 230, 255), SignalGroup.Voltage, 50, 2, True, False))
        r.Add(New RegisterDef(SignalID.P1, "P Active L1", "W", 1.0, -50000, 100000, Color.FromArgb(255, 200, 80), SignalGroup.Power, 52, 2, True, False))
        r.Add(New RegisterDef(SignalID.P2, "P Active L2", "W", 1.0, -50000, 100000, Color.FromArgb(235, 180, 60), SignalGroup.Power, 54, 2, True, False))
        r.Add(New RegisterDef(SignalID.P3, "P Active L3", "W", 1.0, -50000, 100000, Color.FromArgb(215, 160, 40), SignalGroup.Power, 56, 2, True, False))
        r.Add(New RegisterDef(SignalID.Q1, "Q React. L1", "VAR", 1.0, -50000, 100000, Color.FromArgb(255, 130, 80), SignalGroup.Power, 58, 2, True, False))
        r.Add(New RegisterDef(SignalID.Q2, "Q React. L2", "VAR", 1.0, -50000, 100000, Color.FromArgb(235, 110, 60), SignalGroup.Power, 60, 2, True, False))
        r.Add(New RegisterDef(SignalID.Q3, "Q React. L3", "VAR", 1.0, -50000, 100000, Color.FromArgb(215, 90, 40), SignalGroup.Power, 62, 2, True, False))
        r.Add(New RegisterDef(SignalID.S1, "S Apprt. L1", "VA", 1.0, 0, 100000, Color.FromArgb(220, 150, 255), SignalGroup.Power, 64, 2, True, False))
        r.Add(New RegisterDef(SignalID.S2, "S Apprt. L2", "VA", 1.0, 0, 100000, Color.FromArgb(200, 130, 235), SignalGroup.Power, 66, 2, True, False))
        r.Add(New RegisterDef(SignalID.S3, "S Apprt. L3", "VA", 1.0, 0, 100000, Color.FromArgb(180, 110, 215), SignalGroup.Power, 68, 2, True, False))
        r.Add(New RegisterDef(SignalID.PF1, "PF L1", "PF", 100.0, -1, 1, Color.FromArgb(255, 130, 130), SignalGroup.Power, 70, 2, True, False))
        r.Add(New RegisterDef(SignalID.PF2, "PF L2", "PF", 100.0, -1, 1, Color.FromArgb(235, 110, 110), SignalGroup.Power, 72, 2, True, False))
        r.Add(New RegisterDef(SignalID.PF3, "PF L3", "PF", 100.0, -1, 1, Color.FromArgb(215, 90, 90), SignalGroup.Power, 74, 2, True, False))
        r.Add(New RegisterDef(SignalID.DIR1, "Dir L1", "", 1.0, -1.5, 1.5, Color.FromArgb(255, 240, 80), SignalGroup.Phase, 76, 2, False, False))
        r.Add(New RegisterDef(SignalID.DIR2, "Dir L2", "", 1.0, -1.5, 1.5, Color.FromArgb(235, 220, 60), SignalGroup.Phase, 78, 2, False, False))
        r.Add(New RegisterDef(SignalID.DIR3, "Dir L3", "", 1.0, -1.5, 1.5, Color.FromArgb(215, 200, 40), SignalGroup.Phase, 80, 2, False, False))
        r.Add(New RegisterDef(SignalID.UT12, "Angle U1-2", "°", 1.0, -180, 180, Color.FromArgb(200, 255, 200), SignalGroup.Phase, 82, 2, False, False))
        r.Add(New RegisterDef(SignalID.UT23, "Angle U2-3", "°", 1.0, -180, 180, Color.FromArgb(180, 235, 180), SignalGroup.Phase, 84, 2, False, False))
        r.Add(New RegisterDef(SignalID.UT31, "Angle U3-1", "°", 1.0, -180, 180, Color.FromArgb(160, 215, 160), SignalGroup.Phase, 86, 2, False, False))

        'If using TCP-Epack

        If ModbusTcpMaster.modbustcpisconnected Then
            r.Add(New RegisterDef(SignalID.ePack_Vline, "ePack V Line", "V", 100.0, 0, 500, Color.FromArgb(100, 220, 180), SignalGroup.ePack, 256, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Irms, "ePack I rms", "A", 100.0, 0, 100, Color.FromArgb(0, 200, 140), SignalGroup.ePack, 257, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Isqburst, "ePack Iburst", "A", 100.0, 0, 5000, Color.FromArgb(0, 170, 110), SignalGroup.ePack, 258, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Isq, "ePack I", "A", 100.0, 0, 5000, Color.FromArgb(0, 140, 90), SignalGroup.ePack, 259, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Vrms, "ePack Vrms", "V", 100.0, 0, 500, Color.FromArgb(80, 180, 255), SignalGroup.ePack, 260, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Vsq, "ePack V", "V", 100.0, 0, 90000, Color.FromArgb(60, 150, 235), SignalGroup.ePack, 261, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Pburst, "ePack P burst", "W", 100.0, 0, 50000, Color.FromArgb(255, 210, 80), SignalGroup.ePack, 262, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_P, "ePack P", "W", 100.0, 0, 50000, Color.FromArgb(255, 185, 50), SignalGroup.ePack, 263, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_S, "ePack S", "VA", 100.0, 0, 50000, Color.FromArgb(200, 130, 255), SignalGroup.ePack, 264, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_PF, "ePack PF", "PF", 100.0, -1, 1, Color.FromArgb(255, 110, 110), SignalGroup.ePack, 265, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Z, "ePack Z", "Ohm", 100.0, 0, 1000, Color.FromArgb(180, 180, 100), SignalGroup.ePack, 266, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Frequency, "ePack Frequency", "Hz", 10.0, 45, 65, Color.FromArgb(140, 200, 255), SignalGroup.ePack, 267, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Vsqburst, "ePack V burst", "V", 10.0, 0, 90000, Color.FromArgb(40, 120, 215), SignalGroup.ePack, 268, 2, True, False))
            r.Add(New RegisterDef(SignalID.ePack_Ramping, "ePack Ramping", "", 1.0, -1.5, 1.5, Color.FromArgb(255, 150, 150), SignalGroup.ePack, 1440, 2, True, False))

            ' Parametres de configuration ePack (lecture seule, pas de graphique)
            r.Add(New RegisterDef(SignalID.ePack_VNominal, "ePack V Nominal", "V", 1.0, 0, 500, Color.FromArgb(100, 220, 180), SignalGroup.ePack, 3412, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_INominal, "ePack I Nominal", "A", 1.0, 0, 1000, Color.FromArgb(0, 200, 140), SignalGroup.ePack, 3411, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_Firing, "ePack Firing", "", 1.0, 0, 5, Color.FromArgb(255, 185, 50), SignalGroup.ePack, 3402, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_Control, "ePack Control", "", 1.0, 0, 5, Color.FromArgb(255, 100, 50), SignalGroup.ePack, 3405, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_ILimit, "ePack I Limit", "", 1.0, 0, 5, Color.FromArgb(255, 110, 110), SignalGroup.ePack, 3403, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_I2Transfer, "ePack I2 Transf.", "", 1.0, 0, 5, Color.FromArgb(200, 130, 255), SignalGroup.ePack, 3404, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_Xfmr, "ePack Xfmr", "", 1.0, 0, 5, Color.FromArgb(180, 180, 100), SignalGroup.ePack, 3410, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_Heater, "ePack Heater", "", 1.0, 0, 5, Color.FromArgb(255, 150, 80), SignalGroup.ePack, 3406, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_AiFct, "ePack AI Fct", "", 1.0, 0, 5, Color.FromArgb(140, 200, 255), SignalGroup.ePack, 3407, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_AiType, "ePack AI Type", "", 1.0, 0, 5, Color.FromArgb(120, 180, 235), SignalGroup.ePack, 3408, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_Di1Fct, "ePack DI1 Fct", "", 1.0, 0, 5, Color.FromArgb(100, 160, 215), SignalGroup.ePack, 3418, 2, False, False))
            r.Add(New RegisterDef(SignalID.ePack_Di2Fct, "ePack DI2 Fct", "", 1.0, 0, 5, Color.FromArgb(80, 140, 195), SignalGroup.ePack, 3409, 2, False, False))

        End If

        Return r.ToArray()
    End Function

    ' ── Retourne les definition d'un registre par SignalID ───────────────────
    Public Function GetDef(id As SignalID) As RegisterDef
        For Each r As RegisterDef In Registers
            If r.ID = id Then Return r
        Next
        Throw New ArgumentException("Unknown SignalID: " & id.ToString())
    End Function

    ' ── Verifie si un SignalID est present dans les registres charges ─────────
    ' (utile pour les signaux ePack, absents si non connecte)
    Public Function HasDef(id As SignalID) As Boolean
        For Each r As RegisterDef In Registers
            If r.ID = id Then Return True
        Next
        Return False
    End Function

    ' ── Invalide le cache pour forcer un rebuild (ex : connexion/deconnexion ePack)
    Public Sub Refresh()
        _registers = Nothing
    End Sub

    ' ── Liste des signaux temps reel ('true' sur le graphe) ──────────────────
    Public Function GetRealTimeSignals() As List(Of RegisterDef)
        Dim result As New List(Of RegisterDef)
        For Each r As RegisterDef In Registers
            If r.IsRealTime Then result.Add(r)
        Next
        Return result
    End Function

    ' ── Signaux actives par defaut ────────────────────────────────────────────
    Public Function GetDefaultEnabledIDs() As List(Of SignalID)
        Dim result As New List(Of SignalID)
        For Each r As RegisterDef In Registers
            If r.DefaultEnabled Then result.Add(r.ID)
        Next
        Return result
    End Function

    'old'
    '' ── Test Y1 (I et U totaux uniquement) ───────────────────────────────────
    'Public Function IsY1Signal(id As SignalID) As Boolean
    '    Return Registers(CInt(id)).IsY1Candidate
    'End Function

End Module