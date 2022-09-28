using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAM.Core.Services;

namespace PAM.Core.Interfaces;
public interface IAgreementsSearchService
{
  Task<List<AgreementRecord>> GetAllAgreements();
}
