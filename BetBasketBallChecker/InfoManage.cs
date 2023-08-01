using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BetBasketBallChecker
{
    class InfoManage
    {
        List<String> m_phoneList = new List<string>();
        List<String> m_zipList = new List<string>();
        List<String> m_streetList = new List<string>();

        int m_phoneIdx = 0;
        int m_zipIdx = 0;
        int m_streetIdx = 0;

        public InfoManage()
        {
            ReadInfo();
        }

        private void ReadInfo()
        {
            using (StreamReader sr = new StreamReader("./phonelist.json"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    m_phoneList.Add(line);
                }
            }
            using (StreamReader sr = new StreamReader("./ziplist.json"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    m_zipList.Add(line);
                }
            }
            using (StreamReader sr = new StreamReader("./streetlist.json"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    m_streetList.Add(line);
                }
            }
        }

        public String getPhoneNumber()
        {
            if (m_phoneIdx >= m_phoneList.Count)
                m_phoneIdx = 0;
            String ret= m_phoneList.ElementAt(m_phoneIdx);
            m_phoneIdx++;
            return ret;
        }

        public String[] getZipItem()
        {
            if (m_zipIdx >= m_zipList.Count)
                m_zipIdx = 0;
            String ret = m_zipList.ElementAt(m_zipIdx);
            m_zipIdx++;
            return ret.Split(',');
        }

        public String getStreetItem()
        {
            if (m_streetIdx >= m_streetList.Count)
                m_streetIdx = 0;
            String ret= m_streetList.ElementAt(m_streetIdx);
            m_streetIdx++;
            return ret;
        }
    }
}
