using System.Globalization;
using System.Linq;
using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Localisation;
using RPGFramework.Menu;
using RPGFramework.Menu.SharedTypes;
using UnityEngine;

namespace Test
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private TestGlobalInstaller m_GlobalContainer;

        private void Start()
        {
            IEntryPoint entryPoint = CoreModule.Create(m_GlobalContainer);

            ILocalisationService localisationService = new LocalisationService();
            string[]             languages           = localisationService.GetAllLanguages();
            
            CultureInfo[] cultures = languages.Select(CultureInfo.GetCultureInfo).ToArray();
            
            string allCultures = string.Join(", ", cultures.Select(c => c.NativeName));
            
            //TODO: create a "Select Language" screen using the above data, default to platform language if available
            //TODO: store the data in player prefs
            //TODO: set the language on the ILocalisationService object
            Debug.Log(allCultures);
            
            IModuleArgs args = new MenuModuleArgs<IBeginMenu>();

            entryPoint.StartGameAsync<IMenuModule>(args).FireAndForget();
        }
    }
}