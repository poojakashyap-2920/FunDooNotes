using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.IUserEntity
{
    public  interface INotes
    {

        //insert
        public  Task InsertNote(string title, string description, string colour, bool isArchived, bool isDeleted, string emailId);

        //get all record
        public Task<IEnumerable<NotesEntity>> GetNote();

        // get all record by email
        public Task<IEnumerable<NotesEntity>> GetNotesById(int NoteId);


        //delete
        public Task DeleteNoteById(int noteid);

        //update
        public Task UpdateNoteByNoteId(int noteId, NotesEntity notesEntity);



    }
}
