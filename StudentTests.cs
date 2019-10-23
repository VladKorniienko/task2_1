using System;

namespace Korniienko_Task3
{
    class StudentTests : IComparable
    {
        private string _name;
        private string _testName;
        private DateTime _date;
        private double _testResult;

        public StudentTests(string name, string testName, DateTime date, double testResult)
        {
            _name = name;
            _testName = testName;
            _date = date;
            _testResult = testResult;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string TestName
        {
            get => _testName;
            set => _testName = value;
        }

        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        public double TestResult
        {
            get => _testResult;
            set => _testResult = value;
        }

        public int CompareTo(object obj)
        {
            StudentTests stud = obj as StudentTests;
            if (this._testResult == stud._testResult)
            {
                if (this._date > stud._date)
                    return 1;
                if (this._date < stud._date)
                    return -1;
                else
                    return 0;
            }
            if (this._testResult > stud._testResult)
                return 1;
            if (this._testResult < stud._testResult)
                return -1;
            else
                return 0;

        }

        public override string ToString()
        {
            return ($"Topic: {TestName}; Student: {Name}; Date: {Date}; Result: {TestResult}.");
        }
    }
}
