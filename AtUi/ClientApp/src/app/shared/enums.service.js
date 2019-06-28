"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var shipmentStatus;
(function (shipmentStatus) {
    shipmentStatus[shipmentStatus["Uploaded"] = 0] = "Uploaded";
    shipmentStatus[shipmentStatus["Translated"] = 1] = "Translated";
    shipmentStatus[shipmentStatus["Done"] = 2] = "Done";
})(shipmentStatus = exports.shipmentStatus || (exports.shipmentStatus = {}));
var MatStepperTab;
(function (MatStepperTab) {
    MatStepperTab[MatStepperTab["UploadedTab"] = 0] = "UploadedTab";
    MatStepperTab[MatStepperTab["TranslatedTab"] = 1] = "TranslatedTab";
    MatStepperTab[MatStepperTab["SendToSFTab"] = 2] = "SendToSFTab";
    MatStepperTab[MatStepperTab["CompletedTab"] = 3] = "CompletedTab";
})(MatStepperTab = exports.MatStepperTab || (exports.MatStepperTab = {}));
//# sourceMappingURL=enums.service.js.map