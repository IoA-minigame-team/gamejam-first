# 『かぜ、ひいちゃう』

Unity（6000.4.4f1 / URP）製のトップダウン視点サバイバルアクション。unityroom への投稿を前提に、チーム（IoA-minigame-team）でチーム開発。

## 概要

- **ジャンル**: 迫りくる敵を避け続けて生存時間でスコアを稼ぐ、トップダウン型サバイバルアクション
- **基本ルール**:
  - スコアは生存時間に応じて自動加算（`GameManager`）
  - 敵の弾（`Bullet`タグ）に当たると移動速度が落ちる減速デバフ
  - 敵本体・敵の攻撃（`Death`タグ）に触れると即ゲームオーバー
  - フィールドに出現する「マスク」を拾うと一定時間無敵になる
  - ダッシュで敵の攻撃を無敵状態のまま回避できる
- **画面構成**: タイトル → 遊び方 → ゲーム → リザルトの4シーン構成
- **プラットフォーム**: WebGL（unityroom）を主戦場に、モバイルブラウザでは仮想スティックUIが自動表示

## 使用技術

- Unity 6000.4.4f1 / Universal Render Pipeline
- [UniTask](https://github.com/Cysharp/UniTask)（非同期処理）
- [unityroom-client-library](https://github.com/naichilab/unityroom-client-library)（スコア送信・ランキング連携）
- Joystick Pack（モバイル向け仮想スティック）
- Git LFS（バイナリアセットの管理）

## 工夫した点・見てほしい点

- **敵の挙動をコンポーネント合成で組み立てる設計**（`blueprint/EnemyMoveBase` / `EnemyAttackBase`）: 移動ロジックと攻撃ロジックを別コンポーネントに分離し、`EnemyCore`が両方を呼び出すだけの司令塔になっている。組み合わせを変えるだけで新しい敵パターンを追加できる（`EnemyMove_Chase` × `EnemyAttack_Shoot`／`EnemyMove_Lunge` × `EnemyAttack_OmniShot` など）。
- **予告演出付きの敵スポーン**（`EnemySpawnHerald`）: 敵を直接生成せず、まず「予告オブジェクト」を出してから一定時間後に本体を生成することで、プレイヤーに反応の猶予を与えている。
- **スコア連動の動的難易度**（`EnemySpawner`）: `AnimationCurve`でスコアに応じたスポーン間隔の倍率を設定し、時間経過ではなくスコア（＝生存の上手さ）に応じて難易度が上がる作りにした。
- **UniTaskによる非同期の状態管理**: 無敵状態の点滅、減速デバフの解除、リザルト画面への遷移待ちなど、コルーチンで書くと煩雑になる時間差処理を`async/await`で見通し良く実装。
- **シングルトン管理クラス群**（`GameManager` / `BGMManager` / `SEManager`）: `DontDestroyOnLoad`とシーンロードイベントを組み合わせ、シーンをまたいでもBGMやスコアの状態を保持しつつ、シーンに応じて自動でBGMを切り替える。
- **WebGL/モバイル自動判定UI**（`PlatformUIController`）: `UNITY_WEBGL`＋`Application.isMobilePlatform`でモバイルブラウザのみ仮想スティックを表示するなど、プリプロセッサディレクティブで実行環境ごとの出し分けを行っている。
- **Git運用**: バイナリの多いUnityプロジェクトのためGit LFSを導入し、機能ごとにfeatureブランチを切ってPRベースでmainにマージするフローを徹底（`feature/enemy`, `feature/player-movement`など）。

## 改善したい点・未完成な点・既知のバグ

- `Bullet.cs`の`OnTriggerEnter2D`がタグ判定をしておらず、プレイヤー以外の何かに触れても弾が消えてしまう（コード中にも要改修のコメントあり）。
- `BGMManager.cs` / `SEManager.cs`のコメントが文字コードの問題で文字化けしたまま残っている（動作には影響しないが可読性が悪い）。
- `ItemSpawner`は現状マスク1種類のみの出現ロジックで、アイテムの種類を増やす拡張がまだない。
- ダッシュのキー判定漏れ（Shiftキーの片側しか反応しない）やマスクの二重生成など、直近まで細かい不具合修正が続いていたため、同種の考慮漏れが他にも残っている可能性がある。
- 一時停止・音量設定など、プレイ中に呼び出せるオプションUIが未実装。

## プロジェクト構成

```
kaze-hichau/
├─ Assets/
│  ├─ Scenes/        # Title / HowToPlay / Game / Result
│  ├─ Scripts/
│  │  ├─ blueprint/  # 敵AIの抽象基底クラス（Move/Attack）
│  │  ├─ BGM+SE/     # サウンド管理シングルトン
│  │  ├─ Item/       # アイテム（マスク等）関連
│  │  └─ Other Scene/# タイトル・遊び方・リザルトのUI制御
│  └─ Joystick Pack/ # モバイル向け仮想スティック
└─ Packages/
```
