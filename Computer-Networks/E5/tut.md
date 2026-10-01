### Cấu hình SW1

```
enable

configure terminal

hostname SW1

no ip domain-lookup

enable secret Cisco@123

line console 0
password Lab@123
login
logging synchronous
exit

line vty 0 15
password Lab@123
login
exit

banner motd #Thuc Hanh Switch Security#

service password-encryption

interface vlan 1
ip address 192.168.1.3 255.255.255.0
no shutdown
exit

interface f0/1
description Ket noi den PC1
exit
interface f0/24
description Ket noi den SW2
exit
copy running-config startup-config
```

### Cấu hình SW2

```
enable

configure terminal

hostname SW2

no ip domain-lookup

enable secret Cisco@123

line console 0
password Lab@123
login
logging synchronous
exit

line vty 0 15
password Lab@123
login
exit

banner motd #Thuc Hanh Switch Security#

service password-encryption

interface vlan 1
ip address 192.168.1.4 255.255.255.0
no shutdown
exit

interface f0/1
description Ket noi den PC2
interface f0/24
description Ket noi den SW1
exit

copy running-config startup-config
```
3. Để kiểm tra kết nối mạng, sử dụng lệnh ping:
   ```
   ping 192.168.1.1
   ping 192.168.1.2
   ping 192.168.1.3
   ping 192.168.1.4
   ```
## 3. Cấu hình Telnet không cần password

### Trên SW1

```
enable
configure terminal
line vty 0 15
no login
exit
```

### Trên SW2

```
enable
configure terminal
line vty 0 15
no login
exit
```

Để kiểm tra, từ PC kết nối đến Switch:
```
telnet 192.168.1.3
```
hoặc
```
telnet 192.168.1.4
```

## 4. Xem thông tin của các thiết bị

### Trên SW1

```
show cdp entry *
```
### Trên SW2

```
show cdp entry *
```

## 5. Cấu hình SSH trên Switch

### Cấu hình SSH trên SW1

```
enable
configure terminal

ip domain-name cisco.com

crypto key generate rsa
1024

ip ssh version 2

username cisco password Cisco123

line vty 0 15
transport input ssh
login local
exit


copy running-config startup-config
```

### Cấu hình SSH trên SW2

```
enable
configure terminal

ip domain-name cisco.com

crypto key generate rsa
1024

ip ssh version 2

username cisco password Cisco123

line vty 0 15
transport input ssh
login local
exit

copy running-config startup-config
```

### Kiểm tra truy cập từ PC

Từ PC có thể truy cập vào Switch bằng lệnh:
```
ssh -l cisco 192.168.1.3
```
Nhập password: Cisco123

### Kiểm tra từ SW1 đến SW2

Từ SW1 truy cập vào SW2:
```
ssh -l cisco 192.168.1.4
```
Nhập password: Cisco123

## 6. Cấu hình port-security

### Cấu hình port-security trên SW1 (cổng f0/1)

```
enable
configure terminal

interface f0/1

switchport mode access
switchport port-security

switchport port-security maximum 1

switchport port-security mac-address sticky

switchport port-security violation shutdown

exit

copy running-config startup-config
```

### Kiểm tra cấu hình port-security

```
show port-security interface f0/1
show port-security address
```
