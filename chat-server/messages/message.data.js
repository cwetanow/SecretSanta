module.exports = (Message) => {
  return {
    addMessage: (message) => {
      const newMessage = new Message(message);

      return newMessage
        .save();
    },
    getMessages: (room) => {
      return Message.find({ room });
    }
  }
}