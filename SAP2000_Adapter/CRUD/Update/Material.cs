/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.Engine.Structure;
using BH.oM.Structure.MaterialFragments;
using SAP2000v1;
using System.Collections.Generic;

namespace BH.Adapter.SAP2000
{
    public partial class SAP2000Adapter : BHoMAdapter
    {
        /***************************************************/
        /**** Update Material                           ****/
        /***************************************************/

        private bool UpdateObjects(IEnumerable<IMaterialFragment> bhMaterials)
        {
            foreach (IMaterialFragment material in bhMaterials)
            {
                bool success = true;
                eMatType matType = eMatType.NoDesign;
                int colour = 0;
                string guid = null;
                string notes = "";
                if (m_model.PropMaterial.GetMaterial(material.DescriptionOrName(), ref matType, ref colour, ref notes, ref guid) == 0)
                {
                    if (matType != MaterialTypeToCSI(material.IMaterialType()))
                    {
                        Engine.Base.Compute.RecordWarning($"Failed to update material: {material.DescriptionOrName()}, can't update to another material type.");
                        continue;
                    }

                    success &= SetObject(material);
                }
                else
                {
                    // No material of that name found
                    Engine.Base.Compute.RecordWarning($"Failed to update material: {material.DescriptionOrName()}, as no such material was present in the model.");
                }

                if (!success)
                    Engine.Base.Compute.RecordWarning($"Failed to update material: {material.DescriptionOrName()}, all BHoM properties may not have been set.");
            }
            return true;
        }

    }
}

