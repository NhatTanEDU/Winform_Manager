
Dưới đây là tóm tắt các bước để bạn copy vào file ghi chú của mình:

---

### 1. Cách tạo Branch mới (Tránh up trực tiếp vào main)

Mỗi khi làm tính năng mới (ví dụ: sửa giao diện Login), hãy tạo một "nhánh" riêng:

```powershell
# B1: Chuyển về main và cập nhật code mới nhất từ team
git checkout main
git pull origin main

# B2: Tạo và chuyển sang branch mới (ví dụ tên là feature/login-ui)
git checkout -b feature/login-ui

# B3: Kiểm tra xem mình đang ở đúng branch chưa
git branch

```

*Lúc này bạn có thể tha hồ sửa code mà không sợ làm hỏng nhánh main.*

### 2. Cách Up code đã thay đổi lên Git (Push)

Sau khi code xong, bạn đẩy code của branch đó lên server:

```powershell
# B1: Xem các file đã thay đổi
git status

# B2: Thêm các thay đổi vào danh sách chờ
git add .

# B3: Lưu lại với lời nhắn mô tả công việc
git commit -m "Update: Hoàn thành giao diện đăng nhập"

# B4: Đẩy branch này lên GitHub
git push origin feature/login-ui

```

### 3. Cách gộp Branch vào Main (Merge)

Khi tính năng đã chạy tốt và bạn muốn đưa nó vào nhánh chính để đóng gói:

```powershell
# B1: Chuyển về nhánh main
git checkout main

# B2: Cập nhật main một lần nữa cho chắc chắn
git pull origin main

# B3: Gộp code từ branch feature vào main
git merge feature/login-ui

# B4: Đẩy code main sau khi gộp lên GitHub
git push origin main

# B5: (Tùy chọn) Xóa branch phụ sau khi đã gộp xong cho sạch máy
git branch -d feature/login-ui

```

### Mẹo nhỏ cho bạn:

* **Commit nhỏ và thường xuyên:** Đừng đợi code cả tuần mới commit, hãy commit mỗi khi xong một phần nhỏ.
* **Quy tắc đặt tên branch:** Nên đặt theo định dạng `loai/ten-tinh-nang` (ví dụ: `feature/reset-password`, `fix/bug-display`).
* **Lưu ý tại Site:** Nếu bạn đang làm việc onsite tại Vũng Tàu và mạng yếu, hãy thực hiện `git add` và `git commit` ở local trước, khi nào có mạng khỏe thì mới `git push`.