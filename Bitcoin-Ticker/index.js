const express = require('express');
const bodyParser = require('body-parser');
const request = require('request');

const app = express();

app.use(bodyParser.urlencoded({extended: true}));

app.get("/", function(req, res){
    res.sendFile(__dirname + "/index.html");
});

var crypto = req.body.crypto;
var fiat = req.body.fiat;
var name = req.body.amount;
var baseURL = "https://apiv2.bitcoinaverage.com/indices/global/ticker/";
var finalURL = baseURL + crypto + fiat;
var options = {
    url: baseURL,
    method: "GET",
    qs: {
        from: crypto,
        to: fiat,
        amount: amount
    }

}

app.post("/", function(req, res){
    request(options, function(error, response, body){
        var data = JSON.parse(body);
        var price = data.price;
        var currentDate = data.time;
        res.write("The current date is: " + currentDate);
        res.write("<h1> " + amount + crypto + " is currently worth:" + price +  fiat + "</h1>");
        res.send();

    });

});

app.listen(3000, function(){
    console.log("server is running on port 3000");
})