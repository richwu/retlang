using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Mocks;
using NUnit.Framework;
using Retlang;

namespace RetlangTests
{

    [TestFixture]
    public class KeyedBatchSubscriberTests
    {

        [Test]
        public void Batch()
        {
            MockRepository repo = new MockRepository();
            IProcessContext context = repo.CreateMock<IProcessContext>();
            IMessageHeader header = repo.CreateMock<IMessageHeader>();

            ResolveKey<string, int> resolver = delegate(int val)
            {
                return val.ToString();
            };

            KeyedBatchSubscriber<string, int> batch = new KeyedBatchSubscriber<string, int>(resolver, 
                    CheckValues, context, 0);

            context.Enqueue(batch.Flush);

            repo.ReplayAll();

            batch.ReceiveMessage(header, 1);
            batch.ReceiveMessage(header, 2);
            batch.ReceiveMessage(header, 1);
            batch.Flush();
        }

        private void CheckValues(IDictionary<string, IMessageEnvelope<int>> messages)
        {
            Assert.AreEqual(2, messages.Values.Count);
            Assert.AreEqual(1, messages["1"].Message);
            Assert.AreEqual(2, messages["2"].Message);
        }
    }
}