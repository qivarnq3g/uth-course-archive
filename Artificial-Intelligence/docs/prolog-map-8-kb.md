# Prolog Wumpus World — Map 8 và dữ liệu xây dựng KB

## Phạm vi và nguồn

- Lệnh chọn map cố định: `start(8).`
- Chương trình được chạy bằng SWI-Prolog tại `C:\Program Files\swipl\bin\swipl.exe`.
- Nguồn luật và cấu hình: `wumpus-prolog-game-main/wumpus.qlf`; hướng dẫn lệnh: `wumpus-prolog-game-main/README.md`.
- Không sửa project Prolog gốc. Các tệp trong `outputs/` là kết quả dẫn xuất.

Hai lớp thông tin được tách riêng:

1. **Ground truth**: cấu hình thật của map đọc từ predicate `map_data/4` và bảng percept tính bằng các predicate của chương trình.
2. **Tri thức quan sát**: chỉ gồm vị trí, percept và event xuất hiện trong lần chạy thắng.

## Ground truth của map 8

Predicate gốc trả về:

```prolog
map_data(8, (3, 1), (3, 3), [(1, 3), (2, 2), (4, 2)]).
```

Theo thứ tự tham số của `start/1`:

- Kích thước hang: `4 x 4`.
- Cửa hang: `(0,0)`; ô bắt đầu trong hang: `(1,1)`.
- Wumpus: `(3,1)`.
- Gold: `(3,3)`.
- Pit: `(1,3)`, `(2,2)`, `(4,2)`.
- Ban đầu có một mũi tên; Wumpus còn sống; chưa có vàng; điểm `0`.

Sơ đồ, trục `y` tăng từ dưới lên:

```text
y=4 |  .   .   .   .
y=3 |  P   .   G   .
y=2 |  .   P   .   P
y=1 |  S   .   W   .
       x=1 x=2 x=3 x=4

S = start, W = Wumpus, P = pit, G = gold, . = không có hazard/gold
```

## Luật percept dùng cho KB

Với `Adj(x,y)` là bốn ô kề cạnh nằm trong lưới:

```text
Breeze(x,y)  <-> tồn tại (u,v) thuộc Adj(x,y): Pit(u,v)
Stench(x,y)  <-> Wumpus còn sống và tồn tại (u,v) thuộc Adj(x,y): Wumpus(u,v)
Glitter(x,y) <-> Gold(x,y)
Bump         <-> hành động đi vượt biên lưới
Scream       <-> mũi tên giết được Wumpus
```

Sau `shootright` thành công, `Stench` biến mất vì predicate gốc yêu cầu `\+ wumpus_dead`.

## Bảng đầy đủ 16 ô trước khi Wumpus chết

`B`, `S`, `G` lần lượt là Breeze, Stench, Glitter. Dấu `—` nghĩa là không có percept nào trong ba loại này.

| Ô | Nội dung thật | Percept ban đầu | Safe? |
| --- | --- | --- | --- |
| `(1,1)` | Start | — | Có |
| `(2,1)` | Trống | B, S | Có |
| `(3,1)` | Wumpus | — | Không |
| `(4,1)` | Trống | B, S | Có |
| `(1,2)` | Trống | B | Có |
| `(2,2)` | Pit | — | Không |
| `(3,2)` | Trống | B, S | Có |
| `(4,2)` | Pit | — | Không |
| `(1,3)` | Pit | — | Không |
| `(2,3)` | Trống | B | Có |
| `(3,3)` | Gold | G | Có |
| `(4,3)` | Trống | B | Có |
| `(1,4)` | Trống | B | Có |
| `(2,4)` | Trống | — | Có |
| `(3,4)` | Trống | — | Có |
| `(4,4)` | Trống | — | Có |

Percept tại ô hazard trong bảng là giá trị lý thuyết do predicate trả về; agent không thể quan sát bình thường vì bước vào pit hoặc Wumpus còn sống sẽ kết thúc game.

## Lần chạy thắng thực tế

Chuỗi lệnh:

```prolog
start(8).
climbdown.
goright.
shootright.
goright.
goup.
goup.
grab.
godown.
godown.
goleft.
goleft.
climbup.
```

| Bước | Hành động | Vị trí sau hành động | Percept/event | Điểm |
| ---: | --- | --- | --- | ---: |
| 0 | `start(8)` | `(0,0)` | Nothing | 0 |
| 1 | `climbdown` | `(1,1)` | Nothing | 0 |
| 2 | `goright` | `(2,1)` | Breeze, Stench | -1 |
| 3 | `shootright` | `(2,1)` | Scream; sau đó chỉ còn Breeze | -11 |
| 4 | `goright` | `(3,1)` | Nothing | -12 |
| 5 | `goup` | `(3,2)` | Breeze | -13 |
| 6 | `goup` | `(3,3)` | Glitter | -14 |
| 7 | `grab` | `(3,3)` | Có vàng | 86 |
| 8 | `godown` | `(3,2)` | Breeze | 85 |
| 9 | `godown` | `(3,1)` | Nothing | 84 |
| 10 | `goleft` | `(2,1)` | Breeze | 83 |
| 11 | `goleft` | `(1,1)` | Nothing | 82 |
| 12 | `climbup` | `(0,0)` | Hoàn thành nhiệm vụ | **1082** |

Kiểm toán điểm: `8 * (-1) - 10 + 100 + 1000 = 1082`.

## Các mệnh đề KB rút ra từ quan sát

Ký hiệu `Pxy`, `Wxy` là pit và Wumpus tại `(x,y)`.

Từ `Nothing` tại `(1,1)`:

```text
¬P21
¬P12
¬W21
¬W12
```

Từ `Breeze ∧ Stench` tại `(2,1)` và biết `(1,1)` là ô đã đứng an toàn:

```text
P31 ∨ P22
W31 ∨ W22
```

Chỉ các quan sát đến thời điểm này chưa đủ để chứng minh duy nhất `W31`; lần bắn sang phải là hành động kiểm chứng. Từ event `Scream` sau `shootright` tại `(2,1)`:

```text
W31
wumpus_dead
¬Stench(x,y)  (sau thời điểm bắn)
```

Từ việc di chuyển an toàn qua `(3,1)`, `(3,2)`, `(3,3)`:

```text
¬P31, ¬P32, ¬P33
```

Từ `Breeze` tại `(3,2)`:

```text
P22 ∨ P42 ∨ P31 ∨ P33
```

Kết hợp với các ô an toàn đã đi qua:

```text
P22 ∨ P42
```

Từ `Glitter` tại `(3,3)`:

```text
G33
```

Lưu ý: vị trí cả ba pit không thể được chứng minh chỉ từ transcript đường thắng. Các facts `P13`, `P22`, `P42` trong ground truth đến từ `map_data/4`, không phải toàn bộ đều là kết luận quan sát của agent.

## Tệp kết quả

- `outputs/prolog-map-8-kb.pl`: KB Prolog có thể truy vấn, gồm ground truth, luật, quan sát và bảng percept kỳ vọng.
- `outputs/prolog-map-8-transcript.txt`: transcript chuẩn hóa và kiểm toán điểm.

