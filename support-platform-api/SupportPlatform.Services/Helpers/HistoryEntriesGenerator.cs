using SupportPlatform.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Services
{
    public static class HistoryEntriesGenerator
    {
        public static ModificationEntryEntity GetNewResponseEntry(string username, string reportHeading)
        {
            return new ModificationEntryEntity
            {
                Date = DateTime.Now,
                Message = $"Pracownik {username} odpowiedział na zgłoszenie ${reportHeading}"
            };
        }

        public static ModificationEntryEntity GetStatusUpdatedEntry(string username, string statusName)
        {
            return new ModificationEntryEntity
            {
                Date = DateTime.Now,
                Message = $"Pracownik {username} zmienił status zgłoszenia na '{statusName}'"
            };
        }
    }
}
