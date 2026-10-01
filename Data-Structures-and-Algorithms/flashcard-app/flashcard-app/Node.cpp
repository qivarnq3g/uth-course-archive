#include "Node.h"

Node::Node(std::string value) {
    data = value;
    next = nullptr;
    prev = nullptr;
}
