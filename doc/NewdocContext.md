# tạo event

## admin

### Note

- check toàn bộ các số ko đc âm

1. khi tạo event thì ở draft và disable true(hoàn hoàn và chưa hoàn thành các thông tin cơ bản)

- end time > start time > register limit time
- ko đc truyền number round, mặc định là 0
- ko đc để now > start time

2. khi chỉnh sửa event

- ko thể public event khi chưa hoàn thành các thông tin cơ bản
- ko disable thành false khi chưa hoàn thành các thông tin cơ bản

3. round
   3.1 thêm round

- khi tạo 1 ound tự đọng check thứ tự, round No, và round number +1, xóa round cũng như vậy luôn(tự dồn rond No và tính lại round numberr)
- end time của round > start time của round, và phải nằm trong thời gian event
- end time submission round > start time submission round, và phải nằm trong khoảng thời gian của round
  3.2 sửa round
- now > start time round thì ko đc chỉnh sửa round đó
- thay đổi vị trí round, đổi các thông tin round no và thời gian của 2 round đc chọn
- ko thể disable round true/false hoặc chỉnh sửa round khi round đã bắt đầu
- round No 1 mặc định khi tạo hoặc khi đổi thì mặc định limit team bằng limit team cảu event của round đó
- nút chuyển round
  - kết thúc ngay round đó, và set end time của round hiện tại bằng now
  - tự động check round No kế tiếp có tồn tại ko, và limit team là bao nhiêu: lấy ra điểm của total scope(dựa vào điêm trung bình các judge chấm cái submission đó) của mỗi team trong round đó để coi điểm các team và làm bảng tổng kết round, và lấy số lượng team tương ứng với limit team sang round sau, đồng thời sửa lại register team cái round No của mỗi team, và trạng thái bị loại hay chưa
  - ko phân biệt track
- job check now > end round để close round đó

4. critical
   4.1 tạo critical
   - khi tạo thì tạo cả templete và item chung luông, có thể tạo nhiều item thêm trong 1 templete
     4.2 xóa critical
   - khi disable 1 templete thì các item trong nó cũng bị disable theo
     4.3 sửa critical
   - ko cần sửa, tạo thẳng cái mới luôn
   - chọn critical cho 1 event: khi chọn 1 cái templete cho cái round đó (disable false) thì các critical khác trong round đó bị disable true hết, chỉ có cái đc chọn mới false
     note:

- khi round bị disable thì critical của round đó cũng bị disable theo

5. track and topic

- khi disable true/false track thì các topic cũng bị disable true/false theo

6. assign lecture
   6.1 phân công

- phân vào từng event, ko đc cùng làm mentor và judge trong 1 event
- 1 lecture có thể làm mentor cho nhiều event, hoặc làm judge cho nhiều event,
- phân công vào event bị disable đc luôn, lecture đó sẽ thấy được event đc phân công
- phân vào từng track, có thể trong nhiều track
  6.2 xóa
- xóa ra khỏi event là đồng thời xóa ra khỏi các track đc phân công luôn
- xóa ra khỏi track là chỉ ra khỏi số track chỉ định thôi, ko out ra khỏi event

7. award
   7.1 tạo award

- tiêu chí xếp award: lấy level trước, số lượng sau.
- ko thể có 2 award cùng level trong 1 event
- 1 team ko thể có 2 award

8. leaderboard

- quy tắc chấm điểm:
- 1 bài submission trong 1 round của 1 team: tổng các tiêu chí(item)
- điểm của 1 tiêu chí(item) = trung bình cộng các scope item của các judge
- cái đó cũng là điểm của team trong round đó
- điểm của nguyên event = tổng trung bình điểm của các round
- điểm chapter = tổng điểm các event trrong 1 năm
