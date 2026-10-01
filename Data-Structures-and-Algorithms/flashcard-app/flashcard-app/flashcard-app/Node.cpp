#pragma execution_character_set("utf-8")

#include "pch.h"
#include "Node.h"

Node::Node(std::wstring value) {
    data = value;
    next = nullptr;
    prev = nullptr;
}

