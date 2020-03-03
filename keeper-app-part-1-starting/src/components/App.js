import React from "react";
import Header from "./Header";
import Footer from "./Footer";
import Note from "./Note";
import Notes from "../notes.js"

function App() {
  return (
    <div>
      <Header />
        {Notes.map ( newNote =>
            <Note
                key = {newNote.key}
                title = {newNote.title}
                content = {newNote.content}
                />
        )}
      <Footer />
    </div>
  );
}

export default App;
