const dataInit = require('./message.data');
const controllerInit = require('./message.controller');
const routerInit = require('./message.routes');
const Message = require('./message.model');

module.exports = (app) => {
  const data = dataInit(Message);

  const io = require('socket.io')(app.server);

  io.on('connection', (socket) => {
    socket.on('chat message', (msg) => {
      data.addMessage(msg)
        .then((savedMessage) => {
          io.emit('chat message', savedMessage);
        });
    });
  });

  const controller = controllerInit(data);
  const router = routerInit(controller);

  return router;
};