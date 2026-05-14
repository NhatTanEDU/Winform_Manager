🚀 Hướng dẫn Quản lý Git - SongLinhRMA.System
Tài liệu này hướng dẫn cách đồng bộ Repo, cập nhật code và xử lý các tình huống Rollback khi phát triển tính năng feature/frontend-login-ui.

1. Thiết lập ban đầu (Chuyển đổi từ Winform_Manager)
Nếu bạn đang ở thư mục project cũ và muốn trỏ sang Repo mới:

PowerShell
# Kiểm tra remote hiện tại
git remote -v

# Đổi URL sang repo SongLinhRMA
git remote set-url origin https://github.com/NhatTanEDU/SongLinhRMA.System.git

# Tải thông tin mới từ server
git fetch origin

# Chuyển sang branch cần làm việc
git checkout feature/frontend-login-ui
2. Quy trình cập nhật Code hàng ngày
Để tránh xung đột code, hãy thực hiện các lệnh sau trước khi bắt đầu viết code mới:

PowerShell
# 1. Tạm cất các đoạn code đang viết dở
git stash

# 2. Lấy code mới nhất về
git pull origin feature/frontend-login-ui

# 3. Lấy lại code đang viết dở ra để làm tiếp
git stash pop
3. Hướng dẫn Rollback (Quay lại phiên bản cũ)
Dùng khi code mới cập nhật bị lỗi hệ thống hoặc bạn muốn xóa các thay đổi lỗi:

Cách 1: Xóa sạch thay đổi chưa commit (Reset nhanh)
PowerShell
git reset --hard HEAD
Cách 2: Quay về một thời điểm cụ thể trong lịch sử
PowerShell
# Xem danh sách các lần lưu (Commit)
git log --oneline -n 10

# Reset về mã Hash cụ thể (Ví dụ mã là a1b2c3d)
git reset --hard a1b2c3d