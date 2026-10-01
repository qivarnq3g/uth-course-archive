#include "Node.h"
#include "DoublyLinkedList.h"
#include <iostream>
#include <string>

void printMenu() {
    std::cout << "\n=== FLASHCARD APP - DOUBLY LINKED LIST ===\n";
    std::cout << "1. Them the tu vung vao cuoi (append)\n";
    std::cout << "2. Them the tu vung vao dau (prepend)\n";
    std::cout << "3. Xoa the tu vung (deleteNode)\n";
    std::cout << "4. Tim the theo vi tri (findByIndex)\n";
    std::cout << "5. Duyet danh sach xuoi (traverseForward)\n";
    std::cout << "6. Duyet danh sach nguoc (traverseBackward)\n";
    std::cout << "7. Demo day du cac chuc nang\n";
    std::cout << "0. Thoat\n";
    std::cout << "Chon: ";
}

void demoFull(DoublyLinkedList& dll) {
    std::cout << "\n=== DEMO DAY DU CAC CHUC NANG ===\n\n";
    
    std::cout << "1. Them 3 the vao cuoi:\n";
    dll.append("hello");
    dll.append("world");
    dll.append("goodbye");
    std::cout << "   Danh sach sau khi append: ";
    dll.traverseForward();
    
    std::cout << "\n2. Them 1 the vao dau:\n";
    dll.prepend("first");
    std::cout << "   Danh sach sau khi prepend: ";
    dll.traverseForward();
    
    std::cout << "\n3. Duyet xuoi:\n   ";
    dll.traverseForward();
    
    std::cout << "\n4. Duyet nguoc:\n   ";
    dll.traverseBackward();
    
    std::cout << "\n5. Tim the o vi tri 2:\n";
    Node* found = dll.findByIndex(2);
    if (found) {
        std::cout << "   Tim thay: " << found->data << std::endl;
    } else {
        std::cout << "   Khong tim thay\n";
    }
    
    std::cout << "\n6. Xoa the 'world':\n";
    if (dll.deleteNode("world")) {
        std::cout << "   Da xoa thanh cong!\n";
        std::cout << "   Danh sach sau khi xoa: ";
        dll.traverseForward();
    } else {
        std::cout << "   Khong tim thay de xoa\n";
    }
    
    std::cout << "\n7. Duyet nguoc sau khi xoa:\n   ";
    dll.traverseBackward();
    
    std::cout << "\n=== KET THUC DEMO ===\n";
}

int main() {
    DoublyLinkedList dll;
    int choice;
    std::string value;
    int index;
    
    do {
        printMenu();
        std::cin >> choice;
        std::cin.ignore();
        
        switch (choice) {
            case 1: {
                std::cout << "Nhap tu vung can them vao cuoi: ";
                std::getline(std::cin, value);
                dll.append(value);
                std::cout << "Da them '" << value << "' vao cuoi danh sach.\n";
                break;
            }
            case 2: {
                std::cout << "Nhap tu vung can them vao dau: ";
                std::getline(std::cin, value);
                dll.prepend(value);
                std::cout << "Da them '" << value << "' vao dau danh sach.\n";
                break;
            }
            case 3: {
                std::cout << "Nhap tu vung can xoa: ";
                std::getline(std::cin, value);
                if (dll.deleteNode(value)) {
                    std::cout << "Da xoa '" << value << "' thanh cong!\n";
                } else {
                    std::cout << "Khong tim thay '" << value << "' trong danh sach.\n";
                }
                break;
            }
            case 4: {
                std::cout << "Nhap vi tri can tim (bat dau tu 0): ";
                std::cin >> index;
                Node* found = dll.findByIndex(index);
                if (found) {
                    std::cout << "Tim thay tai vi tri " << index << ": " << found->data << std::endl;
                } else {
                    std::cout << "Khong tim thay tai vi tri " << index << std::endl;
                }
                break;
            }
            case 5: {
                std::cout << "Danh sach tu dau den cuoi:\n";
                dll.traverseForward();
                break;
            }
            case 6: {
                std::cout << "Danh sach tu cuoi ve dau:\n";
                dll.traverseBackward();
                break;
            }
            case 7: {
                DoublyLinkedList demoList;
                demoFull(demoList);
                break;
            }
            case 0: {
                std::cout << "Cam on ban da su dung Flashcard App!\n";
                break;
            }
            default: {
                std::cout << "Lua chon khong hop le. Vui long chon lai.\n";
                break;
            }
        }
    } while (choice != 0);
    
    return 0;
}

