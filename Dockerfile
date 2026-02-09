FROM ubuntu:latest


RUN apt-get update && apt-get install -y openssh-server python3 sudo

RUN useradd -rm -d /home/test -s /bin/bash -g root -G sudo test && \
    echo 'test:password' | chpasswd

RUN useradd -rm -d /home/uat -s /bin/bash -g root -G sudo uat && \
    echo 'uat:password' | chpasswd

RUN useradd -rm -d /home/prd -s /bin/bash -g root -G sudo prd && \
    echo 'prd:password' | chpasswd

# Set up the SSH directory
RUN mkdir -p /run/sshd

# Expose Port 22 (The standard SSH port)
EXPOSE 22

# Start the SSH Daemon when the container boots
CMD ["/usr/sbin/sshd", "-D"]