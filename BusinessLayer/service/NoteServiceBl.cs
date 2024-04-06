using BusinessLayer.UserInterface;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.IUserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.service
{
    public class NoteServiceBl : ICobllabBl
    {
        private readonly INotes _note;
        public NoteServiceBl(INotes note)
        { _note = note; }

      

        public Task InsertNote(string title, string description, string colour, bool isArchived, bool isDeleted, string emailId)
        {
            // Call the appropriate method from the _note object to perform the insertion
            return _note.InsertNote(title, description, colour, isArchived, isDeleted, emailId);
        }

        //getnotes
        public Task<IEnumerable<NotesEntity>> GetNote()
        {
           return _note.GetNote();
        }

        //getnote by id
        public Task<IEnumerable<NotesEntity>> GetNotesById(int NoteId)
        {
           return _note.GetNotesById(NoteId);
        }


        //delete Note By Id

        public Task DeleteNoteById(int noteid)
        {
            return _note.DeleteNoteById(noteid);
        }

        //update
        public Task UpdateNoteByNoteId(int noteId, NotesEntity notesEntity)
        {
           return _note.UpdateNoteByNoteId(noteId, notesEntity);
        }
    }
}
