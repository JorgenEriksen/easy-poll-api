﻿namespace EasyPollAPI.DTO
{
    public class QuestionAlternativeDTO
    {
        public int Id { get; set; }
        public string AlternativeText { get; set; }
        public List<int>? usersAnswered { get; set; }
    }
}
