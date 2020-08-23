# caMon
it's a piece of cake Monitor! :)  
BIDS共有メモリに接続し, BVE等から共有されたデータを表示します.

## How to use
BIDS共有メモリが有効化された環境で, caMon.exeを実行するだけです.

## Hardware/Software Requirements
.Net Core 3.0のWPFが使用できる環境であれば, どこでも使用できるはずです.  ARMであれ, x86であれ.  
Windows10 Pro 64bit + Ryzen 2700のマシンで開発およびデバッグを行ったので, 類似の環境であれば動くはずです.  面倒なんでこれ以上の検証は積極的には行いません.

## License
This pproject is under the MIT License.

## Projects in this repository
- caMon
  - スタート画面の実装および, 起点としてSMemの起動/終了等の機能を実装しています.  
  ここから画面表示を実装した各ライブラリを参照します.
- caMon.IPages
  - 画面表示を実装した各ライブラリが備える必要があるインターフェイスを定義しています.
  - 本体側の一部機能を操作するための関数も実装しています.
- caMon.pages.e233sp
  - E233系の速度計画面風表示を実装した, 画面表示ライブラリです.  
  過去に公開していたものを移植して一部修正したものなので, 一部非効率な実装があります.
  - コメントが少なすぎて解読が面倒なので, 余程のバグがない限り手を入れることはしません.
- caMon.pages/e235sp
  - E235系の速度計画面風表示を実装した, 画面表示ライブラリです.  
  過去に公開していたものを移植して一部修正したものなので, 一部非効率な実装があります.
  - コメントが少なすぎて解読が面倒なので, 余程のバグがない限り手を入れることはしません.
