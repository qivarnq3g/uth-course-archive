#include <iostream>
#include <string>
using namespace std;

struct SinhVien
{
    string maSo;
    string hoTen;
    int namSinh;
    float diemTB;
};

struct Node
{
    SinhVien data;
    Node *next;
};

struct DanhSach
{
    Node *head;
};

// a) Khai bao, khoi tao, va nhap danh sach
void khoiTao(DanhSach &ds)
{
    ds.head = NULL;
}

Node *taoNode(SinhVien sv)
{
    Node *p = new Node;
    p->data = sv;
    p->next = NULL;
    return p;
}

void themDau(DanhSach &ds, SinhVien sv)
{
    Node *p = taoNode(sv);
    p->next = ds.head;
    ds.head = p;
}

void nhapSinhVien(SinhVien &sv)
{
    cout << "\nNhap ma so: ";
    cin >> sv.maSo;
    cin.ignore();
    cout << "Nhap ho ten: ";
    getline(cin, sv.hoTen);
    cout << "Nhap nam sinh: ";
    cin >> sv.namSinh;
    cout << "Nhap diem trung binh: ";
    cin >> sv.diemTB;
}

void nhapDanhSach(DanhSach &ds)
{
    int n;
    cout << "\nNhap so luong sinh vien: ";
    cin >> n;
    for (int i = 0; i < n; i++)
    {
        cout << "\n--- Sinh vien thu " << i + 1 << " ---";
        SinhVien sv;
        nhapSinhVien(sv);
        themDau(ds, sv);
    }
}

// b) Duyet va in danh sach
void inSinhVien(SinhVien sv)
{
    cout << sv.maSo << "\t" << sv.hoTen << "\t" << sv.namSinh << "\t" << sv.diemTB << endl;
}

void inDanhSach(DanhSach ds)
{
    if (ds.head == NULL)
    {
        cout << "\nDanh sach rong!" << endl;
        return;
    }
    cout << "\n--- DANH SACH SINH VIEN ---" << endl;
    cout << "Ma so\tHo ten\t\tNam sinh\tDiem TB" << endl;
    Node *p = ds.head;
    while (p != NULL)
    {
        inSinhVien(p->data);
        p = p->next;
    }
}

// c) Tim kiem mot sinh vien theo ho ten
Node *timKiem(DanhSach ds, string hoTen)
{
    Node *p = ds.head;
    while (p != NULL)
    {
        if (p->data.hoTen == hoTen)
        {
            return p;
        }
        p = p->next;
    }
    return NULL;
}

void timKiemTheoTen(DanhSach ds)
{
    string ten;
    cin.ignore();
    cout << "\nNhap ho ten can tim: ";
    getline(cin, ten);
    Node *kq = timKiem(ds, ten);
    if (kq != NULL)
    {
        cout << "\nTim thay sinh vien:" << endl;
        cout << "Ma so\tHo ten\t\tNam sinh\tDiem TB" << endl;
        inSinhVien(kq->data);
    }
    else
    {
        cout << "\nKhong tim thay sinh vien!" << endl;
    }
}

// d) Chen them mot sinh vien moi vao sau mot sinh vien co ma so chi dinh
void chenSauNode(DanhSach &ds, string maSoTruoc, SinhVien svMoi)
{
    Node *p = ds.head;
    while (p != NULL && p->data.maSo != maSoTruoc)
    {
        p = p->next;
    }
    if (p == NULL)
    {
        cout << "\nKhong tim thay ma so " << maSoTruoc << endl;
        return;
    }
    Node *nodeMoi = taoNode(svMoi);
    nodeMoi->next = p->next;
    p->next = nodeMoi;
    cout << "\nDa chen sinh vien moi sau sinh vien co ma so " << maSoTruoc << endl;
}

void chenSinhVien(DanhSach &ds)
{
    string maSoTruoc;
    cout << "\nNhap ma so sinh vien truoc: ";
    cin >> maSoTruoc;
    SinhVien svMoi;
    cout << "\n--- Nhap thong tin sinh vien moi ---";
    nhapSinhVien(svMoi);
    chenSauNode(ds, maSoTruoc, svMoi);
}

// e) Xoa mot sinh vien co ma sinh vien chi dinh
void xoaNode(DanhSach &ds, string maSo)
{
    if (ds.head == NULL)
    {
        cout << "\nDanh sach rong!" << endl;
        return;
    }
    if (ds.head->data.maSo == maSo)
    {
        Node *temp = ds.head;
        ds.head = ds.head->next;
        delete temp;
        cout << "\nDa xoa sinh vien co ma so " << maSo << endl;
        return;
    }
    Node *p = ds.head;
    while (p->next != NULL && p->next->data.maSo != maSo)
    {
        p = p->next;
    }
    if (p->next == NULL)
    {
        cout << "\nKhong tim thay ma so " << maSo << endl;
        return;
    }
    Node *temp = p->next;
    p->next = temp->next;
    delete temp;
    cout << "\nDa xoa sinh vien co ma so " << maSo << endl;
}

void xoaSinhVien(DanhSach &ds)
{
    string maSo;
    cout << "\nNhap ma so sinh vien can xoa: ";
    cin >> maSo;
    xoaNode(ds, maSo);
}

// f) Sap xep danh sach theo ma so sinh vien tang dan
void sapXepTheoMaSo(DanhSach &ds)
{
    if (ds.head == NULL || ds.head->next == NULL)
        return;
    for (Node *i = ds.head; i->next != NULL; i = i->next)
    {
        for (Node *j = i->next; j != NULL; j = j->next)
        {
            if (i->data.maSo > j->data.maSo)
            {
                SinhVien temp = i->data;
                i->data = j->data;
                j->data = temp;
            }
        }
    }
    cout << "\nDa sap xep danh sach theo ma so tang dan!" << endl;
}

// g) Sap xep danh sach theo ten sinh vien theo thu tu bang chu cai A, B, C, ....
void sapXepTheoTen(DanhSach &ds)
{
    if (ds.head == NULL || ds.head->next == NULL)
        return;
    for (Node *i = ds.head; i->next != NULL; i = i->next)
    {
        for (Node *j = i->next; j != NULL; j = j->next)
        {
            if (i->data.hoTen > j->data.hoTen)
            {
                SinhVien temp = i->data;
                i->data = j->data;
                j->data = temp;
            }
        }
    }
    cout << "\nDa sap xep danh sach theo ten!" << endl;
}

// h) Huy danh sach khi ket thuc chuong trinh
void huyDanhSach(DanhSach &ds)
{
    while (ds.head != NULL)
    {
        Node *temp = ds.head;
        ds.head = ds.head->next;
        delete temp;
    }
    cout << "\nDa huy danh sach!" << endl;
}

int main()
{
    DanhSach ds;
    khoiTao(ds);
    int chon;

    do
    {
        cout << "\n========== MENU ==========" << endl;
        cout << "1. Khoi tao va nhap danh sach" << endl;
        cout << "2. In danh sach" << endl;
        cout << "3. Tim kiem sinh vien theo ho ten" << endl;
        cout << "4. Chen sinh vien moi sau mot sinh vien" << endl;
        cout << "5. Xoa sinh vien theo ma so" << endl;
        cout << "6. Sap xep theo ma so tang dan" << endl;
        cout << "7. Sap xep theo ten" << endl;
        cout << "8. Huy danh sach" << endl;
        cout << "0. Thoat" << endl;
        cout << "Chon: ";
        cin >> chon;

        switch (chon)
        {
        case 1:
            huyDanhSach(ds);
            khoiTao(ds);
            nhapDanhSach(ds);
            break;
        case 2:
            inDanhSach(ds);
            break;
        case 3:
            timKiemTheoTen(ds);
            break;
        case 4:
            chenSinhVien(ds);
            break;
        case 5:
            xoaSinhVien(ds);
            break;
        case 6:
            sapXepTheoMaSo(ds);
            break;
        case 7:
            sapXepTheoTen(ds);
            break;
        case 8:
            huyDanhSach(ds);
            khoiTao(ds);
            break;
        case 0:
            cout << "\nTam biet!" << endl;
            break;
        default:
            cout << "\nLua chon khong hop le!" << endl;
        }
    } while (chon != 0);

    huyDanhSach(ds);
    return 0;
}
