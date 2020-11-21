var express = require('express');
var router = express.Router();
var fs = require('fs');


/* GET home page. */
router.get('/', function (req, res, next) {
    let testobj = {'time':'2020', 'text':'mitt fina meddelande att skicka'};
    fs.appendFile('chat.json', ',\n' + JSON.stringify(testobj), function upd() {
        console.log("TODO error if error");
    });
    res.render('index', { title: 'Get Chat' });
});

function isNotWord(message) {
    let messageArr = message.split(/[ ]+/);
    console.log(messageArr);
    let cleanMessageArr = messageArr.map(m => m.replace(/[^\W\s]/gi, ''));

}

module.exports = router;