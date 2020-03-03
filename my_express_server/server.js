const express = require('express');
const app = express();
app.get("/", function(req, res){
   res.send("hello");
});
app.get("/contact", function(req, res){
    res.send("Contact me at: boop@snoop.com");
})
app.get("/about", function(req, res){
    res.send("Hello my name is Loca and I'm a special pug");
})
app.get("/hobbies", function(req, res){
    res.send("Drinking names and kicking beer");
})
app.listen(3000, function(){
    console.log("server started on port 3000");
});