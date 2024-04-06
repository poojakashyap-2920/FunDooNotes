using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.UserInterface
{
    public interface ICobllabBl
    {
        //insert
        public Task InsertNote(string title, string description, string colour, bool isArchived, bool isDeleted, string emailId);
        //get all
        public Task<IEnumerable<NotesEntity>> GetNote();

        //get note by noteid
        public Task<IEnumerable<NotesEntity>> GetNotesById(int NoteId);

        //delete Note by NoteId
        public Task DeleteNoteById(int noteid);

        //update
        public  Task UpdateNoteByNoteId(int noteId, NotesEntity notesEntity);

    }


}
