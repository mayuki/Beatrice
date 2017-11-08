# QuickStart

## Beatrice をクローンして、設定用JSONを作成する
- リポジトリをクローン
    - Raspberry Piを利用している場合には、[Releases](https://github.com/mayuki/Beatrice/releases) ページからビルド済みバイナリをダウンロードしてください
- **Beatrice.Web** ディレクトリに移動します
- **"appsettings.Beatrice.sample.json"** を **"appsettings.Beatrice.json"** としてコピー
- **"actions.sample.json"** を **"actions.json"** としてコピー

## "Actions on Google" プロジェクトを作成する
- [Actions on Google](http://actions.google.com) に移動
- ["Actions Console"](https://console.actions.google.com/) をクリック
- **"Add/import project"** をクリックして新しいプロジェクトを作成
- プロジェクト名の入力と国を選択 (例: MyBeatriceApp と Japan)
- プロジェクトIDをメモしておきます。プロジェクトIDは `Settings` ページかコンソールのURLにあります (例: https://console.actions.google.com/project/mybeatriceapp/ -> mybeatriceapp). 

## 同期リクエスト用にAPIキーを作成する
- [Google Cloud Console API Manager](https://console.developers.google.com/apis) に移動します
- **"Credentials"** をクリック
- Select your project.
- **"Create Credentials"** をクリック
- **"API key"** をクリック
- APIキーをコピーして **"appsettings.Beatrice.json"** の **"SyncRequestApiKey"** 値にペースト 

## ngrok を実行(proxy server)
- ターミナルを開いて `ngrok http 5000 --host-header=localhost` を実行
- 出力される、ngrok上で割り当てられたサブドメイン名をメモ (例: https://example.ngrok.io/ -> example)

## Beatrice の設定 (**appsettings.Beatrice.json**)
- サインインアカウントの設定
    - **Beatrice:Security:User** にユーザー名を設定
    - パスワードを生成します。値は "UserName:Password" という書式の文字列のSHA256ハッシュです
        - Linux/macOS: `echo -n UserName:Password | openssl sha256`
        - Windows: `((New-Object System.Security.Cryptography.SHA256Managed).ComputeHash([System.Text.Encoding]::UTF8.GetBytes("UserName:Password")) | %{ $_.ToString("x2") }) -join ""`
    - **Beatrice:Security:Password** にパスワードを設定
- OAuth 資格情報を設定
    - `ClientId` を作成して **Beatrice:Security:OAuth:ClientId** に設定
    - `ClientSecret` を作成して  **Beatrice:Security:OAuth:ClientSecret** に設定
        - `ClientId` と `ClientSecret` は推測できない任意のランダムな文字列
    - **Beatrice:Security:OAuth:RedirectUrls** の `https://oauth-redirect.googleusercontent.com/r/<ProjectId>` のプレースホルダ`<ProjectId>`をプロジェクトIDで置き換え

## プロジェクトの設定 (Actions on Google)
- Actions プロジェクトへ移動
- "Use Actions SDK" をクリック
- **"actions.json"** のURLをホストされている場所に置き換え
    - `https://<AssignedSubDomain>.ngrok.io/Automation/` -> `https://example.ngrok.io/Automation/`
    - `https://server.home.example.com/Automation/`
    - etc...
- プロジェクト設定を更新するために `gactions` コマンドに**"actions.json"** を渡して実行 
    - [gactions CLIをダウンロード](https://developers.google.com/actions/tools/gactions-cli)
    - `gactions update --project <Project ID> --action_package actions.json` を実行
- "OK" をクリック
- App information の下の "ADD" をクリック
- アプリ情報を入力。例えば、アプリ名や説明、連絡先など
- "Account linking (optional)" をクリックして "ADD" をクリック
- Beatrice の OAuth 設定値をフィールドに入力
    - Grant type: `Authorization code`
    - Client information
        - Client ID: **Beatrice:Security:OAuth:ClientId** と同じ
        - Client secret: **Beatrice:Security:OAuth:ClientSecret** と同じ
        - Authorization URL: `https://<Hosted address>/connect/authorize`
        - Token URL: `https://<Hosted address>/connect/token`
- "Save" をクリック
- "TEST DRAFT" をクリック

## Beatrice を実行
- cd `Beatrice.Web`
- `dotnet run` を実行
    - Raspberry Pi を使っている場合は `./Beatrice.Web` を実行
    - **推奨**: ここで、インターネットからアプリに対して接続できることを確認することをオススメします。例えば ngrok.io を通してアクセスしてサインインしてみます

## Beatrice (プロジェクト) をGoogle Assistantアカウントに接続
- Actions Consoleでプロジェクトを作成したアカウントと同じアカウントでGoogle Assistantでログインしているデバイスを用意
- Google Home か Assistant の設定を開く
- "Home Control" をクリック
- "+" をクリック
- アプリの一覧からプロジェクトを探し、選択
- Beatriceにサインインする
- デバイスが追加されることを確認

## Beatrice の準備完了
Google Assistant(Home) か Console のシミュレータに `"Turn on Outlet1"` (日本語では`アウトレットワンをオンにして`)と話しかけてみましょう。 問題がなければターミナルのログに "Hello! Konnichiwa!" というメッセージが流れます。 

## Optional: Beatrice の設定
- Web サーバー設定
    - デフォルトでは Web サーバーはポート **5000** をリッスンしています。他のポートで動作させたい場合には設定を上書きすることができます
        - `ASPNETCORE_URLS` 環境変数に `http://*:1234` のように設定してください