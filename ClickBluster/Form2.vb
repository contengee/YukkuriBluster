Imports System.Windows.Forms
Public Class F_Main

    Private _rnd As New Random ' ランダム変数
    Private _enemies As New ArrayList() ' 複数の敵を格納する変数
    Public Property Enms As Integer ' 敵の数
    Public Property EnmTim As Integer '敵の動く速度（秒）
    Public Property GmTim As Integer ' 残り時間(秒数)

    ' 難易度初期化
    Public Sub FrmIni()
        ' フォームのサイズでコントロールを配置する
        P_Enemy.Height = Me.Height - P_Enemy.Top - 45
        P_Enemy.Width = Me.Width - 40

        ' 合計得点
        L_Sum.Text = "0"
        ' 敵の動く速度
        T_Enemy.Interval = EnmTim
        ' 残り時間の初期化
        PG_Jikan.Maximum = (GmTim / 1000)
        PG_Jikan.Value = PG_Jikan.Maximum
    End Sub

    Private Sub F_Main_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ' メニュー画面の表示
        F_Menu.Show()
    End Sub

    'スタートボタンクリック
    Private Sub B_Start_Click(sender As Object, e As EventArgs) Handles B_Start.Click
        ' スタートボタンを使えなくする
        B_Start.Enabled = False

        ' 敵の作成
        Dim i As Integer
        ' 敵を10体生成 
        For i = 0 To 9
            ' 配列に敵クラスを生成。その際にオーナーのパネルとランダム変数を渡す
            _enemies.Add(New CEnemy(Me.P_Enemy, _rnd))
            ' 敵のクリックイベント（倒すイベント）
            AddHandler CType(_enemies(i), CEnemy).OnClick, AddressOf EnmOnClick
            ' ループの中でDoEventsを呼んでおく
            Application.DoEvents()
        Next

        ' タイマーのスタート
        T_Enemy.Enabled = True
        T_Jikan.Enabled = True
    End Sub

    Private Sub T_Enemy_Tick(sender As Object, e As EventArgs) Handles T_Enemy.Tick
        Dim i As Integer
        ' 敵の数だけループする
        For i = 0 To _enemies.Count() - 1
            ' 敵の移動イベントを呼ぶ
            CType(_enemies(i), CEnemy).MvEnm()
            ' ループの中でDoEventsを呼んでおく
            Application.DoEvents()
        Next
    End Sub

    Private Sub T_Jikan_Tick(sender As Object, e As EventArgs) Handles T_Jikan.Tick
        If (PG_Jikan.Minimum) <= (PG_Jikan.Value - 1) Then
            ' プログレスバーを一つ下げる
            PG_Jikan.Value = PG_Jikan.Value - 1
        Else
            ' 終わり
            PG_Jikan.Value = PG_Jikan.Minimum
            ' タイマーを止める
            T_Enemy.Enabled = False
            T_Jikan.Enabled = False
            ' メッセージを表示
            MsgBox("終了です" & vbCrLf & "あなたのゆっくり得点は" & L_Sum.Text & "です")
            ' メイン画面を閉じる
            Me.Close()
        End If
    End Sub

    Public Sub EnmOnClick(ByVal sender As Object, ByVal e As EventArgs)
        ' 得点の加算（それぞれに敵の得点を取得する。）
        L_Sum.Text = CInt(L_Sum.Text) + CType(sender, CEnemy).GetTokuten

        ' 効果音を鳴らす
        CType(sender, CEnemy).PlaySound()

        ' 敵を倒したイベント（再度敵を出現させる）
        Call CType(sender, CEnemy).EnemyDown()
    End Sub


End Class