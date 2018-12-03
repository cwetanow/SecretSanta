const mongoose = require('mongoose');

module.exports = (config) => {
  mongoose.Promise = global.Promise;
  mongoose.connect(config.connectionString);

  mongoose.connection.on('error', (err) => {
    console.log(err)
    console.log('MongoDB connection error. Please make sure MongoDB is running.');

    process.exit();
  });
};