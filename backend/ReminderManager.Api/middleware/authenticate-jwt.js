const jwt = require('jsonwebtoken');

const JWT_SECRET = process.env.JWT_SECRET || 'your_jwt_secret_key';

// Dummy DB validation function (replace with real DB logic)
async function isSessionValid(sessionId) {
  // TODO: Replace with actual DB check for session validity
  // Example: return await db.sessions.findOne({ sessionId, isExpired: false, expiry: { $gt: new Date() } });
  return true;
}

function authenticateJWT(req, res, next) {
  // Try to get token from cookie or Authorization header
  let token = null;
  if (req.cookies && req.cookies.jwt) {
    token = req.cookies.jwt;
  } else if (req.headers.authorization && req.headers.authorization.startsWith('Bearer ')) {
    token = req.headers.authorization.split(' ')[1];
  }
  if (!token) {
    return res.status(401).json({ message: 'Authentication required.' });
  }
  jwt.verify(
    token,
    JWT_SECRET,
    {
      issuer: process.env.JWT_ISSUER || 'your_issuer',
      audience: process.env.JWT_AUDIENCE || 'your_audience'
    },
    async (err, decoded) => {
      if (err) return res.status(401).json({ message: 'Invalid or expired token.' });

      req.user = {
        id: decoded['nameid'] || decoded['sub'], // ClaimTypes.NameIdentifier
        name: decoded['name'],                   // ClaimTypes.Name
        email: decoded['email'],                 // ClaimTypes.Email
        sessionId: decoded['sessionid']          // custom claim if present
        // add more if needed
      };

      // Validate sessionId with DB if present
      if (req.user.sessionId) {
        const valid = await isSessionValid(req.user.sessionId);
        if (!valid) {
          return res.status(401).json({ message: 'Session expired or invalid.' });
        }
      }

      next();
    }
  );
}

module.exports = authenticateJWT;
