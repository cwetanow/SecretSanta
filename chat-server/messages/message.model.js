const mongoose = require('mongoose');

const messageSchema = new mongoose.Schema({
  room: {
    type: String,
    required: true
  },
  user: {
    type: String,
    required: true
  },
  content: {
    type: String,
    required: true
  },
  time: {
    type: Date,
    required: true
  },
}, {
    timestamps: true
  });

const Message = mongoose.model('Message', messageSchema);

module.exports = Message;