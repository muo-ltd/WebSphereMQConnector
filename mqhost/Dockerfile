FROM ibmcom/mq:8
RUN useradd testuser -G mqm && \
    echo testuser:Password1 | chpasswd
COPY config.mqsc /etc/mqm/