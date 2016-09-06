using com.ibm.msg.client.jms;
using com.ibm.msg.client.wmq.common;
using javax.jms;
using System;

namespace Mq
{
    public sealed class MqClient : IDisposable
    {
        private string _hostname;
        private int _port;
        private string _channel;
        private string _queueManager;
        private string _targetQueue;

        private Session _session;
        private MessageProducer _producer;
        private Connection _connection;
        private Queue _queue;

        public MqClient(string hostname, int port, string channel, string queueManager, string targetQueue)
        {
            _hostname = hostname;
            _port = port;
            _channel = channel;
            _queueManager = queueManager;
            _targetQueue = targetQueue;

            Open();
        }

        public MqClient(string hostname, string channel, string targetQueue)
            : this(hostname, 1414, channel, string.Empty, targetQueue)
        {
        }

        public void Put(string message)
        {
            var msg = _session.createTextMessage();
            msg.setStringProperty("JMSXGroupID", Guid.NewGuid().ToString());
            msg.setIntProperty("JMSXGroupSeq", 1);
            msg.setBooleanProperty("JMS_IBM_Last_Msg_In_Group", true);
            msg.setText(message);

            _producer.send(msg);
        }

        private MqClient Open()
        {
            var ff = JmsFactoryFactory.getInstance(JmsConstants.__Fields.WMQ_PROVIDER);
            var cf = ff.createConnectionFactory() as JmsConnectionFactory;

            cf.setIntProperty(CommonConstants.__Fields.WMQ_CONNECTION_MODE, CommonConstants.__Fields.WMQ_CM_CLIENT);
            cf.setStringProperty(CommonConstants.__Fields.WMQ_HOST_NAME, _hostname);
            cf.setIntProperty(CommonConstants.__Fields.WMQ_PORT, _port);
            cf.setStringProperty(CommonConstants.__Fields.WMQ_CHANNEL, _channel);
            cf.setStringProperty(CommonConstants.__Fields.WMQ_QUEUE_MANAGER, _queueManager);

            _connection = cf.createConnection();
            _session = _connection.createSession(false, Session.__Fields.AUTO_ACKNOWLEDGE);

            _queue = _session.createQueue("queue:///" + _targetQueue);
            _producer = _session.createProducer(_queue);

            _connection.start();

            return this;
        }

        private MqClient Close()
        {
            if (_producer != null)
                _producer.close();

            if (_session != null)
                _session.close();

            if (_connection != null)
                _connection.close();

            return this;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
