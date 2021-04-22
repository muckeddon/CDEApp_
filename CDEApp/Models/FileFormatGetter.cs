using System.Linq;


namespace CDEApp.Models
{
    public class FileFormatGetter
    {
        //get model, call a GetPositionFormatDot(model) to get a position of last dot and add to it chars of format. Return string with file format like ".png"
        public string GetFileFormat(IAddDoc model)
        {
            string format = " ";

            for (int i = GetPosionOfFormatDot(model); i < model.Document.FileName.ToCharArray().Count(); i++)
            {
                format += model.Document.FileName.ToCharArray()[i];
            }

            return format;
        }

        //get model and return a position of a last dot in name of file
        private int GetPosionOfFormatDot(IAddDoc model)
        {
            int position = 0;

            for (int i = 0; i < model.Document.FileName.ToCharArray().Count(); i++)
            {
                if (model.Document.FileName.ToCharArray()[i] == '.')
                {
                    position = i;
                }
            }

            return position;
        }
    }
}
