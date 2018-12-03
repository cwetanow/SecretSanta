const express = require('express');
const http = require('http');
const cors = require('cors');
const bodyParser = require('body-parser');

const config = require('./config');

const app = express();
app.server = http.createServer(app);

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.use(cors());

require('./config/db')(config);

const messageRouter = require('./messages')(app);
app.use('/message', messageRouter);

app.server.listen(config.port, () => {
  console.log(`Started on port ${config.port}`);
});