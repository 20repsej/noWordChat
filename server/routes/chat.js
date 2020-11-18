var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/get', function (req, res, next) {
    res.render('index', { title: 'Get Chat' });
});
/* GET home page. */
router.get('/set', function (req, res, next) {
    let testmessage = "This is a test";
    isNotWord(testmessage);
    res.render('index', { title: 'Set Chat' });
});
function isNotWord(message) {
    let messageArr = message.split(/[ ]+/);
    console.log(messageArr);
    let cleanMessageArr = messageArr.map(m => m.replace(/[^\W\s]/gi, ''));

}

module.exports = router;