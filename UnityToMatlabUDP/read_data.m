%% MATLAB Unity3D UDP Connection

close all; clear all; clc;

%% Create UDP connection - need one port for reading and another for receiving 
connection = udp('localhost', 8000, 'LocalPort', 8001);
fopen(connection);
disp(connection.status);

% Change the number of GameObjects (has to be done manually!)
GameObjects = 2;

while(1)
    % Read incoming messages from LocalPort
    data_received = fread(connection);
    data_received = native2unicode(data_received, 'UTF-8');
    
    if size(data_received,1) == 4
        data_received = convertCharsToStrings(data_received);
        % Check if the data stream is quitting
        if data_received == 'Quit'
            break
        end
    end
    
    % Turn the data into doubles
    data_received = data_received';
    data_received = strsplit(data_received,',');
    data_received = str2double(data_received);
    
    if size(data_received,2) ~= ((GameObjects * 6) + 2)
        % ignore
    else
        if exist('object_data')
            object_data = [object_data; data_received];
        else
            object_data = data_received;
        end
    end
end

fclose(connection);
disp(connection.status);

%% Save the raw object data to a file

while(1)
    subjectnumber = input('Subject number: '); % Identifier for the subject
    sessionnumber = input('Session number: '); % Identifier for the session (each day is another session)
    trialnumber = input('Trial number: '); % Identifier for the trial
    filename = "RAW_Subject" + int2str(subjectnumber) + "_Session" + int2str(sessionnumber) + "_Trial" + int2str(trialnumber) + ".mat";
    
    if isfile(filename) % Check if filename exists already
        fprintf('\n-----\nData for subject %d exists already. \nPlease change the subject, session and trial numbers below. \n', subjectnumber);
    else
        save(filename,'object_data');
        disp('Data saved as ' + filename);
        break
    end
end