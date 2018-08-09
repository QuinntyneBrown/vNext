using System.Threading.Tasks;
using Xunit;
using vNext.API.Features.Notes;
using vNext.Core.Extensions;
using System;

namespace IntegrationTests.Features
{
    public class NoteScenarios: NoteScenarioBase
    {
        [Fact]
        public async Task ShouldGetNoteById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.GetById(1));

                Assert.True(response.Note.NoteId == 1);
            }
        }

        [Fact]
        public async Task ShouldGetNoteIdbyNoteNoteValue()
        {
            using (var server = CreateServer())
            {
                var note = (await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.GetById(1))).Note;

                var response = await server.CreateClient()
                    .GetAsync<GetNoteIdByNoteQuery.Response>(Get.GetNoteIdByNote(note.Note));

                Assert.True(response.NoteId == note.NoteId);
            }
        }

        [Fact]
        public async Task ShouldNoteGetNoteIdbyNoteNoteValue()
        {
            using (var server = CreateServer())
            {


                var response = await server.CreateClient()
                    .GetAsync<GetNoteIdByNoteQuery.Response>(Get.GetNoteIdByNote($"{Guid.NewGuid()}"));

                Assert.True(response.NoteId == default(int));
            }
        }

        [Fact]
        public async Task ShouldSaveNote()
        {
            using (var server = CreateServer())
            {
                var note = (await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.GetById(1))).Note;

                var response = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes,new SaveNoteCommand.Request() {
                        Note = note
                    });

                Assert.True(response.NoteId == note.NoteId);
            }
        }

        [Fact]
        public async Task ShouldCreateNewNote()
        {
            using (var server = CreateServer())
            {
                var note = (await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.GetById(1))).Note;

                note.Note = null;

                var response = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request()
                    {
                        Note = note
                    });

                Assert.True(response.NoteId != note.NoteId);
            }
        }

    }
}
