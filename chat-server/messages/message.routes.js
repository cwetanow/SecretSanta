const { Router } = require('express');

module.exports = (controller) => {
  const router = new Router();

  router.get('/:id/messages', controller.getMessages);

  return router;
}