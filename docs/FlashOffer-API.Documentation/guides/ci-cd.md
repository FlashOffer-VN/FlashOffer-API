# CI/CD

## Mục đích

Tài liệu này mô tả pipeline CI/CD đang có trong repo và cách mở rộng chúng.

## Workflows hiện tại

### 1. `ci.yml`

- Trigger: Push và Pull Request trên branch `master`.
- Chạy trên `ubuntu-latest`.
- Steps:
  - checkout code
  - setup .NET 9.0
  - restore dependencies
  - build Release
  - test Release với code coverage
  - upload báo cáo coverage

### 2. `docker.yml`

- Trigger: Push trên branch `master` và tạo tag `v*`.
- Chạy trên `ubuntu-latest`.
- Steps:
  - checkout code
  - setup Docker Buildx
  - login Docker Hub bằng secrets
  - build image và push với tag `latest` và `${{ github.sha }}`

## Secrets cần cấu hình

- `DOCKER_USERNAME` - Docker Hub username
- `DOCKER_TOKEN` - Docker Hub access token

## Mở rộng CI/CD

### Thêm môi trường staging

- Copy `ci.yml` thành `ci-staging.yml`.
- Thay trigger branch thành `staging`.
- Cập nhật nếu cần release stage khác.

### Thêm deploy Azure

Nếu cần deploy Azure Web App, thêm workflow mới với `azure/webapps-deploy@v2`.

### Đổi tên branch chính

Nếu repo dùng branch khác `main`/`master`, cập nhật trigger của workflow tương ứng.

## Lưu ý

- Workflow `docker.yml` hiện chỉ build và push image; không deploy tự động.
- Nếu bạn dùng Docker Compose local thì workflow này không thay đổi cấu hình local.
