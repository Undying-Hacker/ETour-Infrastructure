﻿using Core.Entities;
using Core.Interfaces;
using Infrastructure.InterfaceImpls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    [AllowAnonymous]
    public class QAHub : Hub
    {
        public static readonly string PATH = "/qa";
        private readonly ICustomerRepository _customerRepository;
        public QAHub(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task sendQuestions(string authorID, string title, string category)
        {
            Customer customer = await _customerRepository.FindAsync(authorID);

            Question.QuestionCategory? type;

            Question question = null ;

            if (int.TryParse(category, out int ordinal) && (type = (Question.QuestionCategory?)ordinal) != null)
            {
                 question = new Question
                {
                    Owner = customer,
                    
                    Title = title,
                    Category = type.Value,
                    LastUpdated = DateTime.UtcNow,
                    Priority = Question.QuestionPriority.Low,
                    Status = Question.QuestionStatus.Pending
                };
            }



            await Clients.All.SendAsync("ReceiveQuestion",
                question.Owner.Name,
                question.Title,
                question.Category.ToString(),
                question.LastUpdated,
                question.Priority.ToString(),
                question.Status.ToString());
        }

    }
}

