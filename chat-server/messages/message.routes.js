const { Router } = require('express');

module.exports = (controller) => {
  const router = new Router();

  router.get('/:id', controller.getMessages);

  return router;
}