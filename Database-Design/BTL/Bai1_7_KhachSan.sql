CREATE TABLE khach_hang (
    khach_hang_id SERIAL PRIMARY KEY,
    ho_ten VARCHAR(100) NOT NULL,
    so_dien_thoai VARCHAR(20),
    email VARCHAR(100),
    dia_chi TEXT,
    ngay_sinh DATE,
    gioi_tinh VARCHAR(10) CHECK (gioi_tinh IN ('Nam', 'Nữ', 'Khác') OR gioi_tinh IS NULL),
    CONSTRAINT chk_email_format CHECK (email IS NULL OR email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$')
);

CREATE TABLE phong (
    phong_id SERIAL PRIMARY KEY,
    so_phong VARCHAR(10) UNIQUE NOT NULL,
    loai_phong VARCHAR(50) NOT NULL,
    don_gia NUMERIC(12,2) NOT NULL CHECK (don_gia >= 0),
    tinh_trang VARCHAR(20) NOT NULL CHECK (tinh_trang IN ('Có sẵn', 'Đang sử dụng', 'Đang bảo trì', 'Đã đặt')),
    tang INT CHECK (tang > 0),
    ghi_chu TEXT
);

CREATE TABLE tien_nghi (
    tien_nghi_id SERIAL PRIMARY KEY,
    ten_tien_nghi VARCHAR(100) NOT NULL,
    mo_ta TEXT
);

CREATE TABLE phong_tien_nghi (
    phong_id INT REFERENCES phong(phong_id) ON DELETE CASCADE,
    tien_nghi_id INT REFERENCES tien_nghi(tien_nghi_id) ON DELETE CASCADE,
    PRIMARY KEY (phong_id, tien_nghi_id)
);

CREATE TABLE dat_phong (
    dat_phong_id SERIAL PRIMARY KEY,
    khach_hang_id INT REFERENCES khach_hang(khach_hang_id) ON DELETE RESTRICT NOT NULL,
    phong_id INT REFERENCES phong(phong_id) ON DELETE RESTRICT NOT NULL,
    ngay_dat DATE NOT NULL DEFAULT CURRENT_DATE,
    ngay_nhan DATE,
    ngay_tra DATE,
    so_nguoi INT CHECK (so_nguoi > 0),
    trang_thai VARCHAR(30) NOT NULL CHECK (trang_thai IN ('Đã đặt', 'Đã nhận phòng', 'Đã trả phòng', 'Đã hủy')),
    ghi_chu TEXT,
    CONSTRAINT chk_ngay_dat_phong CHECK (
        (ngay_nhan IS NULL OR ngay_nhan >= ngay_dat) AND
        (ngay_tra IS NULL OR ngay_nhan IS NULL OR ngay_tra >= ngay_nhan)
    )
);

CREATE TABLE nhan_vien (
    nhan_vien_id SERIAL PRIMARY KEY,
    ho_ten VARCHAR(100) NOT NULL,
    chuc_vu VARCHAR(50),
    so_dien_thoai VARCHAR(20),
    email VARCHAR(100),
    luong NUMERIC(12,2) CHECK (luong >= 0),
    ngay_vao_lam DATE,
    CONSTRAINT chk_nhan_vien_email_format CHECK (email IS NULL OR email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$')
);

CREATE TABLE nhiem_vu (
    nhiem_vu_id SERIAL PRIMARY KEY,
    ten_nhiem_vu VARCHAR(100) NOT NULL,
    mo_ta TEXT
);

CREATE TABLE nhan_vien_nhiem_vu (
    nhan_vien_id INT REFERENCES nhan_vien(nhan_vien_id) ON DELETE CASCADE,
    nhiem_vu_id INT REFERENCES nhiem_vu(nhiem_vu_id) ON DELETE CASCADE,
    ngay_giao DATE,
    ghi_chu TEXT,
    PRIMARY KEY (nhan_vien_id, nhiem_vu_id, ngay_giao)
);

CREATE TABLE dich_vu (
    dich_vu_id SERIAL PRIMARY KEY,
    ten_dich_vu VARCHAR(100) NOT NULL,
    don_gia NUMERIC(12,2) NOT NULL CHECK (don_gia >= 0),
    don_vi_tinh VARCHAR(20),
    mo_ta TEXT
);

CREATE TABLE su_dung_dich_vu (
    su_dung_dich_vu_id SERIAL PRIMARY KEY,
    dat_phong_id INT REFERENCES dat_phong(dat_phong_id) ON DELETE CASCADE NOT NULL,
    dich_vu_id INT REFERENCES dich_vu(dich_vu_id) ON DELETE RESTRICT NOT NULL,
    so_luong INT NOT NULL CHECK (so_luong > 0),
    don_gia NUMERIC(12,2) NOT NULL CHECK (don_gia >= 0),
    thoi_gian_su_dung TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE hoa_don (
    hoa_don_id SERIAL PRIMARY KEY,
    dat_phong_id INT REFERENCES dat_phong(dat_phong_id) ON DELETE RESTRICT NOT NULL,
    khach_hang_id INT REFERENCES khach_hang(khach_hang_id) ON DELETE RESTRICT NOT NULL,
    ngay_lap DATE NOT NULL DEFAULT CURRENT_DATE,
    tong_tien NUMERIC(14,2) NOT NULL CHECK (tong_tien >= 0),
    phuong_thuc_thanh_toan VARCHAR(30),
    trang_thai VARCHAR(30) NOT NULL CHECK (trang_thai IN ('Chưa thanh toán', 'Đã thanh toán', 'Đã hủy')),
    ghi_chu TEXT
);

CREATE INDEX idx_dat_phong_khach_hang ON dat_phong(khach_hang_id);
CREATE INDEX idx_dat_phong_phong ON dat_phong(phong_id);
CREATE INDEX idx_su_dung_dich_vu_dat_phong ON su_dung_dich_vu(dat_phong_id);
CREATE INDEX idx_su_dung_dich_vu_dich_vu ON su_dung_dich_vu(dich_vu_id);
CREATE INDEX idx_hoa_don_dat_phong ON hoa_don(dat_phong_id);
CREATE INDEX idx_hoa_don_khach_hang ON hoa_don(khach_hang_id);
CREATE INDEX idx_nhan_vien_nhiem_vu_nhan_vien ON nhan_vien_nhiem_vu(nhan_vien_id);
CREATE INDEX idx_nhan_vien_nhiem_vu_nhiem_vu ON nhan_vien_nhiem_vu(nhiem_vu_id);
CREATE INDEX idx_phong_tien_nghi_phong ON phong_tien_nghi(phong_id);
CREATE INDEX idx_phong_tien_nghi_tien_nghi ON phong_tien_nghi(tien_nghi_id);

CREATE INDEX idx_khach_hang_email ON khach_hang(email) WHERE email IS NOT NULL;
CREATE INDEX idx_khach_hang_so_dien_thoai ON khach_hang(so_dien_thoai) WHERE so_dien_thoai IS NOT NULL;
CREATE INDEX idx_phong_so_phong ON phong(so_phong);
CREATE INDEX idx_phong_tinh_trang ON phong(tinh_trang);
CREATE INDEX idx_dat_phong_trang_thai ON dat_phong(trang_thai);
CREATE INDEX idx_dat_phong_ngay_nhan ON dat_phong(ngay_nhan);
CREATE INDEX idx_hoa_don_trang_thai ON hoa_don(trang_thai);
CREATE INDEX idx_hoa_don_ngay_lap ON hoa_don(ngay_lap);
