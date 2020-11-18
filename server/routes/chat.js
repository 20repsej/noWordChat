var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/get', function(req, res, next) {
  res.render('index', { title: 'Get Chat' });
});
/* GET home page. */
router.get('/set', function(req, res, next) {
  res.render('index', { title: 'Set Chat' });
});



module.exports = router;