using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Entities;

namespace WebApplication1
{
    public class MeetupSeeder
    {
        private readonly MeetupContext _meetupContext;
        public MeetupSeeder(MeetupContext meetupContext)
        {
            _meetupContext = meetupContext;
        }

        public void Seed()
        {
            if (_meetupContext.Database.CanConnect())
            {
                if (!_meetupContext.Meetups.Any())
                {
                    InsertSimpleData();
                }
            }
        }

        private void InsertSimpleData()
        {
            var meetups = new List<Meetup>
            {
                new Meetup
                {
                    Name = "Web Submit",
                    Date = DateTime.Now.AddDays(7),
                    IsPrivate = false,
                    Organizer = "Microsoft",
                    Location = new Location()
                    {
                        City = "Nowa Sól",
                        Street = "Matejki 22",
                        PostCode = "67-100"
                    },
                    Lectures = new List<Lecture>
                    {
                        new Lecture
                        {
                            Author = "Bob Clark",
                            Topic = "Modern Browsers",
                            Description = "Deep dive into V8"
                        }

                    }
                },

                new Meetup
                {
                    Name = "4Devs",
                    Date = DateTime.Now.AddDays(3),
                    IsPrivate = false,
                    Organizer = "Microsoft",
                    Location = new Location()
                    {
                        City = "Wroclaw",
                        Street = "Matejki 22",
                        PostCode = "00-007"
                    },
                    Lectures = new List<Lecture>
                    {
                        new Lecture
                        {
                            Author = "Bob Clark",
                            Topic = "Modern Browsers",
                            Description = "Deep dive into V10 Porsche"
                        },
                        new Lecture
                        {
                            Author = "Lukasz Zietek",
                            Topic = "Swiat i Kredki",
                            Description = "Wejscie w swiat 5G"
                        }

                    }
                }
            };

            _meetupContext.AddRange(meetups);
            _meetupContext.SaveChanges();
        }
    }
}
