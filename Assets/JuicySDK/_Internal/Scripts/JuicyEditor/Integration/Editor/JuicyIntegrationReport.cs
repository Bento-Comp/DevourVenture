using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JuicyInternal
{
    public class JuicyIntegrationReportItem
    {
        public string report;
        public string fix;
        public bool isError;

        public JuicyIntegrationReportItem(string report, bool isError, string fix = "")
        {
            this.report = report;
            this.isError = isError;
            this.fix = fix;
        }
    }

    public class JuicyIntegrationReportCategory
    {
        public string Name { get; private set; }
        public List<JuicyIntegrationReportItem> items = new List<JuicyIntegrationReportItem>();
        public bool isEmpty
        {
            get
            {
                return items.Count == 0;
            }
        }

        public int errorAmount
        {
            get
            {
                return items.Count(i => i.isError);
            }
        }

        public int warningAmount
        {
            get
            {
                return items.Count(i => !i.isError);
            }
        }

        public JuicyIntegrationReportCategory(string name)
        {
            Name = name;
            items = new List<JuicyIntegrationReportItem>();
        }

        public JuicyIntegrationReportCategory(string name, List<JuicyIntegrationReportItem> items)
        {
            Name = name;
            this.items = items;
        }

        public void Add(JuicyIntegrationReportItem item)
        {
            items.Add(item);
        }
    }

    public class JuicyIntegrationReport
    {
        protected List<JuicyIntegrationReportCategory> categories = new List<JuicyIntegrationReportCategory>();
        public List<JuicyIntegrationReportCategory> Categories { get { return categories; } }

        public int warningAmount
        {
            get
            {
                int amount = 0;
                foreach (JuicyIntegrationReportCategory category in categories)
                    amount += category.warningAmount;
                return amount;
            }
        }

        public int errorAmount
        {
            get
            {
                int amount = 0;
                foreach (JuicyIntegrationReportCategory category in categories)
                    amount += category.errorAmount;
                return amount;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Categories.Count == 0;
            }
        }
    }
}
