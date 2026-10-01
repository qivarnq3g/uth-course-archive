#pragma execution_character_set("utf-8")

#include "pch.h"
#include "DoublyLinkedList.h"
#include <iostream>
#include <sstream>
#include <iomanip>

DoublyLinkedList::DoublyLinkedList() {
    head = nullptr;
    tail = nullptr;
    currentIndex = -1;
    currentNode = nullptr;
}

DoublyLinkedList::~DoublyLinkedList() {
    Node* current = head;
    while (current) {
        Node* next = current->next;
        delete current;
        current = next;
    }
    head = nullptr;
    tail = nullptr;
}

void DoublyLinkedList::append(std::wstring value) {
    Node* newNode = new Node(value);

    if (!head) {
        head = tail = newNode;
        currentIndex = 0;
        currentNode = head;
        return;
    }

    tail->next = newNode;
    newNode->prev = tail;
    tail = newNode;
}

void DoublyLinkedList::prepend(std::wstring value) {
    Node* newNode = new Node(value);

    if (!head) {
        head = tail = newNode;
        currentIndex = 0;
        currentNode = head;
        return;
    }

    newNode->next = head;
    head->prev = newNode;
    head = newNode;
    
    if (currentIndex >= 0) {
        currentIndex++;
    }
}

bool DoublyLinkedList::insertAt(int index, std::wstring value) {
    if (index < 0) return false;
    
    if (index == 0) {
        prepend(value);
        return true;
    }
    
    Node* current = head;
    int i = 0;
    
    while (current && i < index - 1) {
        current = current->next;
        i++;
    }
    
    if (!current) {
        if (i == index - 1) {
            append(value);
            return true;
        }
        return false;
    }
    
    Node* newNode = new Node(value);
    newNode->next = current->next;
    newNode->prev = current;
    
    if (current->next) {
        current->next->prev = newNode;
    } else {
        tail = newNode;
    }
    
    current->next = newNode;
    
    if (currentIndex >= index) {
        currentIndex++;
    }
    
    return true;
}

bool DoublyLinkedList::deleteNode(std::wstring value) {
    Node* current = head;
    int index = 0;

    while (current) {
        if (current->data == value) {
            if (current == head) head = current->next;
            if (current == tail) tail = current->prev;

            if (current->prev) current->prev->next = current->next;
            if (current->next) current->next->prev = current->prev;

            if (current == currentNode) {
                if (current->next) {
                    currentNode = current->next;
                } else if (current->prev) {
                    currentNode = current->prev;
                    currentIndex--;
                } else {
                    currentNode = nullptr;
                    currentIndex = -1;
                }
            } else if (index < currentIndex) {
                currentIndex--;
            }

            delete current;
            return true;
        }
        current = current->next;
        index++;
    }
    return false;
}

bool DoublyLinkedList::deleteAt(int index) {
    Node* node = findByIndex(index);
    if (!node) return false;
    return deleteNode(node->data);
}

Node* DoublyLinkedList::search(std::wstring value) {
    Node* current = head;
    while (current) {
        if (current->data == value) {
            return current;
        }
        current = current->next;
    }
    return nullptr;
}

Node* DoublyLinkedList::findByIndex(int index) {
    Node* current = head;
    int i = 0;

    while (current) {
        if (i == index) {
            currentIndex = index;
            currentNode = current;
            return current;
        }
        current = current->next;
        i++;
    }
    return nullptr;
}

void DoublyLinkedList::traverseForward() {
    Node* current = head;
    while (current) {
        std::wcout << current->data << L" ";
        current = current->next;
    }
    std::wcout << std::endl;
}

void DoublyLinkedList::traverseBackward() {
    Node* current = tail;
    while (current) {
        std::wcout << current->data << L" ";
        current = current->prev;
    }
    std::wcout << std::endl;
}

int DoublyLinkedList::traverseForwardCount() {
    int count = 0;
    Node* current = head;
    while (current) {
        count++;
        current = current->next;
    }
    return count;
}

int DoublyLinkedList::traverseBackwardCount() {
    int count = 0;
    Node* current = tail;
    while (current) {
        count++;
        current = current->prev;
    }
    return count;
}

int DoublyLinkedList::getSize() {
    int count = 0;
    Node* current = head;
    while (current) {
        count++;
        current = current->next;
    }
    return count;
}

Node* DoublyLinkedList::getCurrent() {
    return currentNode;
}

void DoublyLinkedList::setCurrentIndex(int index) {
    Node* found = findByIndex(index);
    if (found) {
        currentIndex = index;
        currentNode = found;
    }
}

int DoublyLinkedList::getCurrentIndex() {
    return currentIndex;
}

std::wstring DoublyLinkedList::getAllCardsAsString() {
    std::wostringstream oss;
    Node* current = head;
    int index = 0;
    
    if (!head) {
        return L"Danh sách trống";
    }
    
    while (current) {
        oss << index << L". " << current->data;
        if (current == currentNode) {
            oss << L" ← (hiện tại)";
        }
        oss << L"\n";
        current = current->next;
        index++;
    }
    
    return oss.str();
}

std::wstring DoublyLinkedList::getAllCardsAsStringReverse() {
    std::wostringstream oss;
    Node* current = tail;
    int index = getSize() - 1;
    
    if (!tail) {
        return L"Danh sách trống";
    }
    
    while (current) {
        oss << index << L". " << current->data;
        if (current == currentNode) {
            oss << L" ← (hiện tại)";
        }
        oss << L"\n";
        current = current->prev;
        index--;
    }
    
    return oss.str();
}

bool DoublyLinkedList::updateCurrent(const std::wstring& newValue) {
    if (currentNode == nullptr) {
        return false;
    }
    currentNode->data = newValue;
    return true;
}

bool DoublyLinkedList::deleteCurrent() {
    if (currentNode == nullptr) {
        return false;
    }
    
    Node* node = currentNode;
    
    if (node == head) {
        head = node->next;
    }
    if (node == tail) {
        tail = node->prev;
    }
    
    if (node->prev != nullptr) {
        node->prev->next = node->next;
    }
    if (node->next != nullptr) {
        node->next->prev = node->prev;
    }
    
    if (node->next != nullptr) {
        currentNode = node->next;
    } else if (node->prev != nullptr) {
        currentNode = node->prev;
        currentIndex--;
    } else {
        currentNode = nullptr;
        currentIndex = -1;
    }
    
    delete node;
    return true;
}

