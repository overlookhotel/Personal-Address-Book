using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project___personal_address_book
{
    public partial class Form1 : Form
    {
        // declare array
        contactInfo[] contactArray = new contactInfo[100];

        // declare int for counter
        int iCurrentIndex = 0;

        // TO ASK MS EMAMI!!!
        // - should i change user input into title case so that if they mess up when searching, a result will still appear?

        // TO REMEMBER
        // don't forget to test for lowercase inputs / discrepancy
        
        public struct contactInfo
        {
            public string FirstName;
            public string LastName;
            public string PhoneNumber;
            public string EmailAddress;
            public string Instagram;
            public int BirthdayDay;
            public int BirthdayMonth;
            public int BirthdayYear;
        }

        bool bEditMode;

        public Form1()
        {
            InitializeComponent();

            bEditMode = false;
        }
        
        public void ResetAddPage()
        {
            txtAddFirstName.Clear();
            txtAddLastName.Clear();
            txtAddPhoneNumber.Clear();
            txtAddEmail.Clear();
            txtAddInstagram.Clear();
            dtpAddBirthday.Value = DateTime.Now;
        }

        public void ResetShowPage()
        {
            txtShowFirstName.Clear();
            txtShowLastName.Clear();
            txtShowPhoneNumber.Clear();
            txtShowEmail.Clear();
            txtShowInstagram.Clear();
            dtpShowBirthday.Value = DateTime.Now;
        }

        public void ResetEDPage()
        {
            txtEDFirstName.Clear();
            txtEDLastName.Clear();
            txtEDPhoneNumber.Clear();
            txtEDEmail.Clear();
            txtEDInstagram.Clear();
            dtpEDBirthday.Value = DateTime.Now;
            gbEDEditMode.Visible = false;
            txtEDFirstName.Enabled = true;
            txtEDLastName.Enabled = true;
            btnEdit.Enabled = true;
            bEditMode = false;
        }

        public void ResetAll()
        {
            ResetAddPage();
            ResetShowPage();
            lbShowTodaysBirthdays.Items.Clear();
            ResetEDPage();
        }

        public int SearchForName(string strFirstName, string strLastName)
        {
            if (strFirstName == "" || strLastName == "")
            {
                MessageBox.Show("Please input a name for both first and last names.");
                return -2;
            }
          
            for (int i = 0; i < iCurrentIndex; i++)
            {
                if (strFirstName.ToLower() == contactArray[i].FirstName.ToLower() && strLastName.ToLower() == contactArray[i].LastName.ToLower())
                {
                    return i;
                }
            }

            return -1;
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            try
            {
                // put all the inputs into variables, trim
                string strFirstName, strLastName, strPhoneNumber, strEmail, strInstagram;
                int iBirthdayDay, iBirthdayMonth, iBirthdayYear, iSearchIndex;

                strFirstName = txtAddFirstName.Text.Trim();
                strLastName = txtAddLastName.Text.Trim();
                strPhoneNumber = txtAddPhoneNumber.Text.Trim();
                strEmail = txtAddEmail.Text.Trim();
                strInstagram = txtAddInstagram.Text.Trim();

                iBirthdayDay = dtpAddBirthday.Value.Day;
                iBirthdayMonth = dtpAddBirthday.Value.Month;
                iBirthdayYear = dtpAddBirthday.Value.Year;

                // check for all required input are not unreasonable
                if (strFirstName == "" || strLastName == "" || strPhoneNumber == "" || strEmail == "")                   
                {
                    MessageBox.Show("One or more required input(s) is/are missing. Please check over the form and resubmit.");
                    return;
                }  
                if (dtpAddBirthday.Value >= DateTime.Now)
                {
                    MessageBox.Show("Error! The date chosen is in the future. Please change it.");
                    dtpAddBirthday.Value = DateTime.Now;
                    return;
                }

                // search database to make sure that name does not exist
                iSearchIndex = SearchForName(strFirstName, strLastName);
                if (iSearchIndex == -1)
                {
                    // add inputs to the array
                    contactArray[iCurrentIndex].FirstName = strFirstName;
                    contactArray[iCurrentIndex].LastName = strLastName;
                    contactArray[iCurrentIndex].PhoneNumber = strPhoneNumber;
                    contactArray[iCurrentIndex].EmailAddress = strEmail;
                    contactArray[iCurrentIndex].Instagram = strInstagram;
                    contactArray[iCurrentIndex].BirthdayDay = iBirthdayDay;
                    contactArray[iCurrentIndex].BirthdayMonth = iBirthdayMonth;
                    contactArray[iCurrentIndex].BirthdayYear = iBirthdayYear;

                    // update iCurrentIndex
                    iCurrentIndex += 1;

                    MessageBox.Show("Contact successfully added to personal address book");
                    ResetAddPage();
                }
                else
                {
                    // let user know person already exists
                    MessageBox.Show("Apologies, this person already exists in the contact book. Please edit contact if information needs to be changed");

                    ResetAddPage();
                }  
            }
            catch
            {
                MessageBox.Show("Error (code 1)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void btnAddClear_Click(object sender, EventArgs e)
        {
            ResetAddPage();
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // declare variables
                int iSearchIndex, iYear, iMonth, iDay;
                string strFirstName, strLastName;

                strFirstName = txtEDFirstName.Text.Trim();
                strLastName = txtEDLastName.Text.Trim();

                // search for the person
                iSearchIndex = SearchForName(strFirstName, strLastName);

                if (iSearchIndex == -1)
                {
                    MessageBox.Show("The contact you are looking for cannot be found. Please try again.");
                    txtEDFirstName.Clear();
                    txtEDLastName.Clear();
                    return;
                }
                else if (iSearchIndex == -2)
                {
                    return;
                }
                else
                {
                    //disable access to other tabs/functionality when in edit mode
                    bEditMode = true;
                    gbEDEditMode.Visible = true;
                    btnEdit.Enabled = false;
                    txtEDFirstName.Enabled = false;
                    txtEDLastName.Enabled = false;

                    txtEDPhoneNumber.Text = contactArray[iSearchIndex].PhoneNumber;
                    txtEDEmail.Text = contactArray[iSearchIndex].EmailAddress;
                    txtEDInstagram.Text = contactArray[iSearchIndex].Instagram;

                    iYear = contactArray[iSearchIndex].BirthdayYear;
                    iMonth = contactArray[iSearchIndex].BirthdayMonth;
                    iDay = contactArray[iSearchIndex].BirthdayDay;

                    dtpEDBirthday.Value = new DateTime(iYear, iMonth, iDay);
                }
            }
            catch
            {
                MessageBox.Show("Error (code 2)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }

        }

        private void btnShowContact_Click(object sender, EventArgs e)
        {
            try
            {
                string strFirstName, strLastName;
                int iSearchIndex, iYear, iMonth, iDay;
                
                // make birthday invisible
                dtpShowBirthday.Visible = false;
                lblBirthday.Visible = false;

                // get the first and last name
                strFirstName = txtShowFirstName.Text.Trim();
                strLastName = txtShowLastName.Text.Trim();

                iSearchIndex = SearchForName(strFirstName, strLastName);

                if (iSearchIndex == -2)
                {
                    ResetShowPage();
                    return;
                }
                else if (iSearchIndex == -1)
                {
                    MessageBox.Show("The person you are searching for is not in your contacts. Please try again.");
                    ResetShowPage();
                    return;                    
                }
                else
                {
                    // make contact info visibile
                    lblPhoneNumber.Visible = true;
                    txtShowPhoneNumber.Visible = true;
                    lblEmailAddress.Visible = true;
                    txtShowEmail.Visible = true;
                    lblInstagram.Visible = true;
                    txtShowInstagram.Visible = true;

                    // output to textboxes
                    txtShowPhoneNumber.Text = contactArray[iSearchIndex].PhoneNumber;
                    txtShowEmail.Text = contactArray[iSearchIndex].EmailAddress;
                    txtShowInstagram.Text = contactArray[iSearchIndex].Instagram;

                    iYear = contactArray[iSearchIndex].BirthdayYear;
                    iMonth = contactArray[iSearchIndex].BirthdayMonth;
                    iDay = contactArray[iSearchIndex].BirthdayDay;

                    dtpEDBirthday.Value = new DateTime(iYear, iMonth, iDay);
                }
            }
            catch
            {
                MessageBox.Show("Error (code 3)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void btnShowBirthday_Click(object sender, EventArgs e)
        {
            try
            {
                // make contact info invisible
                lblPhoneNumber.Visible = false;
                txtShowPhoneNumber.Visible = false;
                lblEmailAddress.Visible = false;
                txtShowEmail.Visible = false;
                lblInstagram.Visible = false;
                txtShowInstagram.Visible = false;

                string strFirstName, strLastName;
                int iSearchIndex, iYear, iMonth, iDay;

                // populate variables
                strFirstName = txtShowFirstName.Text.Trim();
                strLastName = txtShowLastName.Text.Trim();

                iSearchIndex = SearchForName(strFirstName, strLastName);

                if (iSearchIndex == -2)
                {
                    ResetShowPage();
                    return;
                }
                else if (iSearchIndex == -1)
                {
                    MessageBox.Show("The person you are searching for is not in your contacts. Please try again.");
                    ResetShowPage();
                    return;
                }
                else
                {
                    // make birthday visible
                    dtpShowBirthday.Visible = true;
                    lblBirthday.Visible = true;

                    iYear = contactArray[iSearchIndex].BirthdayYear;
                    iMonth = contactArray[iSearchIndex].BirthdayMonth;
                    iDay = contactArray[iSearchIndex].BirthdayDay;

                    dtpShowBirthday.Value = new DateTime(iYear, iMonth, iDay);
                }
            }
            catch
            {
                MessageBox.Show("Error (code 4)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strName;
                int iNumBirthdays = 0;

                // if in edit mode, ensure tab is always 3
                if (bEditMode == true)
                {
                    TabControl.SelectedIndex = 3;
                    return;
                }

                // clear tab every time it changes
                ResetAddPage();
                ResetShowPage();
                lbShowTodaysBirthdays.Items.Clear();
                ResetEDPage();

                // if tab is open
                if (TabControl.SelectedIndex == 2)
                {
                    for (int i = 0; i < iCurrentIndex; i++)
                    {
                        // check if birthday is today
                        if (contactArray[i].BirthdayMonth == DateTime.Now.Month && contactArray[i].BirthdayDay == DateTime.Now.Day)
                        {
                            // output to list box
                            strName = contactArray[i].FirstName + " " + contactArray[i].LastName;
                            lbShowTodaysBirthdays.Items.Add(strName);
                            iNumBirthdays++;
                        }
                    }
                    // let user know if there are no birthdays today
                    if (iNumBirthdays == 0)
                    {
                        lbShowTodaysBirthdays.Items.Add("No birthdays today");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error (code 5)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void btnEDConfirmEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // ensure that all inputs are still valid
                string strNumber, strEmail, strInstagram, strFirstName, strLastName;
                int iDay, iMonth, iYear, iSearchIndex;

                strNumber = txtEDPhoneNumber.Text.Trim();
                strEmail = txtEDEmail.Text.Trim();
                strInstagram = txtEDInstagram.Text.Trim();

                strFirstName = txtEDFirstName.Text.Trim();
                strLastName = txtEDLastName.Text.Trim();

                if (strNumber == "" || strEmail == "" || strInstagram == "")
                {
                    MessageBox.Show("Please ensure that none of the boxes are blank.");
                    return;
                }

                iDay = dtpEDBirthday.Value.Day;
                iMonth = dtpEDBirthday.Value.Month;
                iYear = dtpEDBirthday.Value.Year;

                // save info
                iSearchIndex = SearchForName(strFirstName, strLastName);

                contactArray[iSearchIndex].PhoneNumber = strNumber;
                contactArray[iSearchIndex].EmailAddress = strEmail;
                contactArray[iSearchIndex].Instagram = strInstagram;
                contactArray[iSearchIndex].BirthdayYear = iYear;
                contactArray[iSearchIndex].BirthdayMonth = iMonth;
                contactArray[iSearchIndex].BirthdayDay = iDay;

                MessageBox.Show("Information has been updated!");
                ResetEDPage();
            }
            catch
            {
                MessageBox.Show("Error (code 6)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void btnEDCancel_Click(object sender, EventArgs e)
        {
            ResetEDPage();
        }

        private void btnEDDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // get index of person that needs to be deleted in array
                int iPersonIndex;
                string strFirstName, strLastName;

                strFirstName = txtEDFirstName.Text.Trim();
                strLastName = txtEDLastName.Text.Trim();

                iPersonIndex = SearchForName(strFirstName, strLastName);

                // using a for loop, move every person from above down AND change iCurrentIndex
                for (int i = iPersonIndex; i < iCurrentIndex; i++)
                {
                    contactArray[i].FirstName = contactArray[i + 1].FirstName;
                    contactArray[i].LastName = contactArray[i + 1].LastName;
                    contactArray[i].PhoneNumber = contactArray[i + 1].PhoneNumber;
                    contactArray[i].EmailAddress = contactArray[i + 1].EmailAddress;
                    contactArray[i].Instagram = contactArray[i + 1].Instagram;
                    contactArray[i].BirthdayDay = contactArray[i + 1].BirthdayDay;
                    contactArray[i].BirthdayMonth = contactArray[i + 1].BirthdayMonth;
                    contactArray[i].BirthdayYear = contactArray[i + 1].BirthdayYear;
                }

                iCurrentIndex--;

                // user message
                MessageBox.Show("Contact has been successfully deleted.");
                ResetEDPage();
            }
            catch
            {
                MessageBox.Show("Error (code 7)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void saveTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string strFileName;
                SaveFileDialog saveDialog1 = new SaveFileDialog();

                saveDialog1.Filter = "txt files (*.txt)|*.txt|All Files (*.*)|*.*";
                
                if (saveDialog1.ShowDialog() == DialogResult.OK)
                {
                    strFileName = saveDialog1.FileName;
                    TextWriter tw = new StreamWriter(strFileName);
                    
                    // save everything into text tile
                    for (int i = 0; i < iCurrentIndex; i++)
                    {
                        tw.Write(contactArray[i].FirstName + "~");
                        tw.Write(contactArray[i].LastName + "~");
                        tw.Write(contactArray[i].PhoneNumber + "~");
                        tw.Write(contactArray[i].EmailAddress + "~");
                        tw.Write(contactArray[i].Instagram + "~");
                        tw.Write(contactArray[i].BirthdayDay.ToString() + "~");
                        tw.Write(contactArray[i].BirthdayMonth.ToString() + "~");
                        tw.WriteLine(contactArray[i].BirthdayYear.ToString());
                    }

                    tw.Close();
                    MessageBox.Show("Successfully saved as text file");
                }
            }
            catch
            {
                MessageBox.Show("Error (code 8)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void openTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openDialog1 = new OpenFileDialog();

                string strFileName, strInput;
                string[] splittedInput;

                if (openDialog1.ShowDialog() == DialogResult.OK)
                {
                    strFileName = openDialog1.FileName;
                    TextReader tr = new StreamReader(strFileName);
                    iCurrentIndex = 0;
                    while ((strInput = tr.ReadLine()) != null)
                    {
                        splittedInput = strInput.Split('~');
                        contactArray[iCurrentIndex].FirstName = splittedInput[0];
                        contactArray[iCurrentIndex].LastName = splittedInput[1];
                        contactArray[iCurrentIndex].PhoneNumber = splittedInput[2];
                        contactArray[iCurrentIndex].EmailAddress = splittedInput[3];
                        contactArray[iCurrentIndex].Instagram = splittedInput[4];
                        contactArray[iCurrentIndex].BirthdayDay = Convert.ToInt32(splittedInput[5]);
                        contactArray[iCurrentIndex].BirthdayMonth = Convert.ToInt32(splittedInput[6]);
                        contactArray[iCurrentIndex].BirthdayYear = Convert.ToInt32(splittedInput[7]);
                        iCurrentIndex++;
                    }

                    tr.Close();
                    ResetAll();
                    MessageBox.Show("Text file successfully opened");
                }
            }
            catch
            {
                MessageBox.Show("Error (code 9)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void saveBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog2 = new SaveFileDialog();
                string strFileName;

                saveDialog2.Filter = "binary files (*.bnf)|*.bnf|All files (*.*|*.*";
                if (saveDialog2.ShowDialog() == DialogResult.OK)
                {
                    strFileName = saveDialog2.FileName;
                    FileStream fs = new FileStream(strFileName, FileMode.Create);
                    BinaryWriter binWriter = new BinaryWriter(fs);

                    for (int i = 0; i < iCurrentIndex; i++)
                    {
                        binWriter.Write(contactArray[i].FirstName);
                        binWriter.Write(contactArray[i].LastName);
                        binWriter.Write(contactArray[i].PhoneNumber);
                        binWriter.Write(contactArray[i].EmailAddress);
                        binWriter.Write(contactArray[i].Instagram);
                        binWriter.Write(contactArray[i].BirthdayDay);
                        binWriter.Write(contactArray[i].BirthdayMonth);
                        binWriter.Write(contactArray[i].BirthdayYear);
                    }

                    binWriter.Flush();
                    binWriter.Close();
                    MessageBox.Show("Successfully saved as binary file");
                }
            }
            catch
            {
                MessageBox.Show("Error (code 10)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }

        private void openBinaryFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openDialog2 = new OpenFileDialog();
                string strFileName;
                long lLength;

                openDialog2.Filter = "binary files (*.bnf)|*.bnf|All files (*.*)|*.*";

                if (openDialog2.ShowDialog() == DialogResult.OK)
                {
                    strFileName = openDialog2.FileName;
                    FileStream fs = new FileStream(strFileName, FileMode.Open);
                    BinaryReader binReader = new BinaryReader(fs);

                    iCurrentIndex = 0;
                    lLength = binReader.BaseStream.Length;
                    while (fs.Position < lLength)
                    {
                        contactArray[iCurrentIndex].FirstName = binReader.ReadString();
                        contactArray[iCurrentIndex].LastName = binReader.ReadString();
                        contactArray[iCurrentIndex].PhoneNumber = binReader.ReadString();
                        contactArray[iCurrentIndex].EmailAddress = binReader.ReadString();
                        contactArray[iCurrentIndex].Instagram = binReader.ReadString();
                        contactArray[iCurrentIndex].BirthdayDay = binReader.ReadInt32();
                        contactArray[iCurrentIndex].BirthdayMonth = binReader.ReadInt32();
                        contactArray[iCurrentIndex].BirthdayYear = binReader.ReadInt32();
                        iCurrentIndex++;
                    }

                    binReader.Close();
                    ResetAll();
                    MessageBox.Show("Binary file successfully opened");
                }
            }
            catch
            {
                MessageBox.Show("Error (code 11)! Please try again at a later time and contact customer service for support");
                ResetAll();
            }
        }
    }
}
