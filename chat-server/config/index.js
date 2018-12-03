require('dotenv').config();

const config = {
  env: process.env.NODE_ENV,
  port: process.env.PORT,
  connectionString: process.env.CONNECTION_STRING
};

console.log(config);

module.exports = config;